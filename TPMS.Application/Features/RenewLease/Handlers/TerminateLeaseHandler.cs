using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Leases.Commands;
using TPMS.Application.Features.RenewLease.Commands;
using TPMS.Domain.Entities;
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
            // ---------------------------------------------------
            // 1️1. Load Lease + Financials
            // ---------------------------------------------------
            var lease = await _db.Leases
                .Include(l => l.RentSchedules)
                .Include(l => l.DepositMaster)
                .FirstOrDefaultAsync(
                    l => l.LeaseID == request.LeaseID,
                    cancellationToken);

            if (lease == null)
                throw new InvalidOperationException("Lease not found.");

            if (lease.IsTerminated)
                throw new InvalidOperationException("Lease already terminated.");

            // ---------------------------------------------------
            // 2️2. Calculate Outstanding Rent
            // ---------------------------------------------------
            decimal outstandingRent = lease.RentSchedules
                .Where(rs => !rs.IsPaid && rs.DueDate <= request.EffectiveEndDate)
                .Sum(rs => rs.Amount);

            // ---------------------------------------------------
            // 3️3. Deposit Calculations
            // ---------------------------------------------------
            decimal depositPaid = lease.DepositMaster?.PaidAmount ?? lease.Deposit;

            decimal depositAdjusted =
                outstandingRent +
                request.PenaltyAmount +
                request.DamageCharges;

            decimal depositRefunded =
                Math.Max(0, depositPaid - depositAdjusted);

            // ---------------------------------------------------
            // 4. Create LeaseTermination Record
            // ---------------------------------------------------
            var termination = new LeaseTermination
            {
                LeaseID = lease.LeaseID,
                TerminationDate = request.TerminationDate,
                EffectiveEndDate = request.EffectiveEndDate,
                TerminationType = request.TerminationType,
                TerminationReason = request.TerminationReason,

                OutstandingRent = outstandingRent,
                PenaltyAmount = request.PenaltyAmount,
                DamageCharges = request.DamageCharges,

                DepositAdjusted = depositAdjusted,
                DepositRefunded = depositRefunded,

                SettlementStatus = "Pending",
                CreatedBy = request.CreatedBy
            };

            _db.LeaseTerminations.Add(termination);

            // ---------------------------------------------------
            // 5 Update Lease
            // ---------------------------------------------------
            lease.IsTerminated = true;
            lease.TerminatedAt = request.EffectiveEndDate;
            lease.Status = "Terminated";
            lease.EndDate = request.EffectiveEndDate;
            lease.EndReason = request.TerminationReason;
            lease.UpdatedAt = DateTime.UtcNow;

            _db.Leases.Update(lease);

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

            // ---------------------------------------------------
            // 7 Update Deposit Master
            // ---------------------------------------------------
            if (lease.DepositMaster != null)
            {
                lease.DepositMaster.BalanceAmount = depositRefunded;
                lease.DepositMaster.Status =
                    depositRefunded > 0 ? "Refund Pending" : "Settled";
            }

            // ---------------------------------------------------
            // 8 Save
            // ---------------------------------------------------
            await _db.SaveChangesAsync(cancellationToken);

            return termination.LeaseTerminationID;
        }
    }
}
