using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Services;

public class LeaseSettlementService : ILeaseSettlementService
{
    private readonly TPMSDBContext _db;

    public LeaseSettlementService(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<LeaseSettlement> CreateSettlementAsync(
        int leaseId,
        decimal penaltyAmount,
        decimal damageCharges,
        CancellationToken cancellationToken)
    {
        var lease = await _db.Leases
            .Include(l => l.RentSchedules)
            .Include(l => l.DepositMaster)
            .FirstOrDefaultAsync(l => l.LeaseID == leaseId, cancellationToken);

        if (lease == null)
            throw new InvalidOperationException("Lease not found.");

        if (lease.Status != LeaseStatus.Expired &&
            lease.Status != LeaseStatus.Terminated)
            throw new InvalidOperationException("Settlement allowed only for expired or terminated leases.");

        var calculation = CalculateSettlement(lease, penaltyAmount, damageCharges);

        var settlement = new LeaseSettlement
        {
            LeaseId = lease.LeaseID,
            SettlementDate = DateTime.UtcNow,

            OutstandingRent = calculation.OutstandingRent,
            DepositPaid = calculation.DepositPaid,
            DepositAdjusted = calculation.DepositAdjusted,
            DepositRefunded = calculation.DepositRefunded,
            BalancePayableByTenant = calculation.BalancePayableByTenant,

            PenaltyAmount = penaltyAmount,
            DamageCharges = damageCharges,

            Status = SettlementStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _db.LeaseSettlements.Add(settlement);
        await _db.SaveChangesAsync(cancellationToken);

        return settlement;
    }

    public async Task CompleteSettlementAsync(
        int settlementId,
        CancellationToken cancellationToken)
    {
        var settlement = await _db.LeaseSettlements
            .Include(s => s.Lease)
            .FirstOrDefaultAsync(s => s.LeaseSettlementId == settlementId, cancellationToken);

        if (settlement == null)
            throw new InvalidOperationException("Settlement not found.");

        settlement.Status = SettlementStatus.Settled;

        // Optional: close lease after settlement
        settlement.Lease.Status = LeaseStatus.Closed;

        await _db.SaveChangesAsync(cancellationToken);
    }

    private SettlementCalculationResult CalculateSettlement(
        Lease lease,
        decimal penaltyAmount,
        decimal damageCharges)
    {
        var unpaidSchedules = lease.RentSchedules
            .Where(r => !r.IsPaid)
            .ToList();

        decimal outstandingRent = unpaidSchedules.Sum(r => r.Amount);

        decimal depositPaid =
            lease.DepositMaster?.PaidAmount ?? lease.Deposit;

        decimal depositAdjusted =
            outstandingRent + penaltyAmount + damageCharges;

        decimal depositRefunded =
            Math.Max(0, depositPaid - depositAdjusted);

        decimal balancePayable =
            depositAdjusted > depositPaid
                ? depositAdjusted - depositPaid
                : 0;

        return new SettlementCalculationResult
        {
            OutstandingRent = outstandingRent,
            DepositPaid = depositPaid,
            DepositAdjusted = depositAdjusted,
            DepositRefunded = depositRefunded,
            BalancePayableByTenant = balancePayable
        };
    }
}
