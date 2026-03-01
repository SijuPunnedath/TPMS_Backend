
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RenewLease.Commands;
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class TerminateLeaseHandler
    : IRequestHandler<TerminateLeaseCommand, int>
{
    private readonly TPMSDBContext _db;
    public TerminateLeaseHandler(TPMSDBContext db)
    {
        _db = db;
    }

public async Task<int> Handle(
    TerminateLeaseCommand request,
    CancellationToken cancellationToken)
{
    using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

    // ---------------------------------------------------
    // 1 Load Lease + Financials + Property
    // ---------------------------------------------------
    var lease = await _db.Leases
        .Include(l => l.RentSchedules)
        .Include(l => l.DepositMaster)
        .FirstOrDefaultAsync(
            l => l.LeaseID == request.LeaseID,
            cancellationToken);

    if (lease == null)
        throw new InvalidOperationException("Lease not found.");

    if (lease.Status != LeaseStatus.Active)
        throw new InvalidOperationException("Only active leases can be terminated.");

    if (lease.IsTerminated)
        throw new InvalidOperationException("Lease already terminated.");

    var property = await _db.Properties
        .FirstOrDefaultAsync(p => p.PropertyID == lease.PropertyID, cancellationToken);

    if (property == null)
        throw new InvalidOperationException("Property not found.");

    // ===================================================
    //  NEW BUSINESS RULE VALIDATION
    // ===================================================
    if (lease.LeaseType == LeaseType.Inbound &&
        property.ActiveOutboundLeaseId != null)
    {
        throw new InvalidOperationException(
            "Cannot terminate inbound lease while outbound lease is active. Terminate outbound lease first.");
    }

    // ---------------------------------------------------
    // 2 Calculate Outstanding Rent
    // ---------------------------------------------------
    var unpaidSchedules = lease.RentSchedules
        .Where(rs => !rs.IsPaid && rs.DueDate <= request.EffectiveEndDate)
        .ToList();

    decimal outstandingRent = unpaidSchedules.Sum(rs => rs.Amount);

    // ---------------------------------------------------
    // 3 Deposit Calculations
    // ---------------------------------------------------
    decimal depositPaid = lease.DepositMaster?.PaidAmount ?? lease.Deposit;

    decimal depositAdjusted =
        outstandingRent +
        request.PenaltyAmount +
        request.DamageCharges;

    decimal depositRefunded =
        Math.Max(0, depositPaid - depositAdjusted);

    // ---------------------------------------------------
    // 4 Create Termination Record
    // ---------------------------------------------------
    var termination = new LeaseTermination
    {
        LeaseID = lease.LeaseID,
        TerminationDate = request.TerminationDate,
        EffectiveEndDate = request.EffectiveEndDate,
        TerminationType = request.TerminationType,
        TerminationReason = request.TerminationReason,

        SettlementStatus = "Pending",
        CreatedBy = request.CreatedBy,
        CreatedAt = DateTime.UtcNow
    };

    _db.LeaseTerminations.Add(termination);

    // ---------------------------------------------------
    // 5 Update Lease State
    // ---------------------------------------------------
    lease.IsTerminated = true;
    lease.Status = LeaseStatus.Terminated;
    lease.TerminatedAt = request.EffectiveEndDate;
    lease.EndDate = request.EffectiveEndDate;
    lease.EndReason = request.TerminationReason;
    lease.UpdatedAt = DateTime.UtcNow;

    // ---------------------------------------------------
    // 6 Cancel Future Rent Schedules
    // ---------------------------------------------------
    var futureSchedules = lease.RentSchedules
        .Where(rs => rs.DueDate > request.EffectiveEndDate)
        .ToList();

    foreach (var schedule in futureSchedules)
    {
        schedule.IsDeleted = true;
    }

    foreach (var schedule in unpaidSchedules)
    {
        schedule.IsClosed = true;
    }

    // ---------------------------------------------------
// 7 Clear Property Active Lease
// ---------------------------------------------------
    if (lease.LeaseType == LeaseType.Inbound)
    {
        // Clear inbound
        property.ActiveInboundLeaseId = null;

        // At this point outbound must already be null
        // (we validated earlier that inbound cannot be terminated if outbound exists)

        property.Status = PropertyStatus.Blocked;
    }
    else // Outbound termination
    {
        // Clear outbound
        property.ActiveOutboundLeaseId = null;

        // No outbound tenant = Vacant (even if inbound still active)
        property.Status = PropertyStatus.Vacant;
    }

    // ---------------------------------------------------
    // 8 Update Deposit Master
    // ---------------------------------------------------
    if (lease.DepositMaster != null)
    {
        lease.DepositMaster.BalanceAmount = depositRefunded;
        lease.DepositMaster.Status =
            depositRefunded > 0 ? "Refund Pending" : "Settled";
    }

    // ---------------------------------------------------
    // 9 Save Everything
    // ---------------------------------------------------
    await _db.SaveChangesAsync(cancellationToken);
    await transaction.CommitAsync(cancellationToken);

    return termination.LeaseTerminationID;
}
}

}

