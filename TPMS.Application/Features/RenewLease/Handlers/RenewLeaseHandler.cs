//namespace TPMS.Application.Features.RenewLease.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Leases.Commands;
using TPMS.Application.Features.RenewLease.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class RenewLeaseHandler
        : IRequestHandler<RenewLeaseCommand, int>
    {
        private readonly TPMSDBContext _db;

        public RenewLeaseHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<int> Handle(
            RenewLeaseCommand request,
            CancellationToken cancellationToken)
        {
            // ---------------------------------------------------
            // 1️⃣ Load Lease
            // ---------------------------------------------------
            var lease = await _db.Leases
                .Include(l => l.RentSchedules)
                .FirstOrDefaultAsync(
                    l => l.LeaseID == request.LeaseID,
                    cancellationToken);

            if (lease == null)
                throw new InvalidOperationException("Lease not found.");

            if (lease.IsTerminated)
                throw new InvalidOperationException("Cannot renew a terminated lease.");

            if (lease.Status != "Active")
                throw new InvalidOperationException("Only active leases can be renewed.");

            if (request.NewEndDate <= lease.EndDate)
                throw new InvalidOperationException("New end date must be after current end date.");

            // ---------------------------------------------------
            // 2️⃣ Create Renewal Record
            // ---------------------------------------------------
            var renewal = new LeaseRenewal
            {
                LeaseID = lease.LeaseID,
                OldEndDate = lease.EndDate,
                NewStartDate = request.NewStartDate,
                NewEndDate = request.NewEndDate,
                OldRent = lease.Rent,
                NewRent = request.NewRent,
                OldDeposit = lease.Deposit,
                NewDeposit = request.NewDeposit,
                AdditionalDeposit = request.NewDeposit.HasValue
                    ? request.NewDeposit.Value - lease.Deposit
                    : null,
                RenewalReason = request.RenewalReason,
                RenewedBy = request.RenewedBy
            };

            _db.LeaseRenewals.Add(renewal);

            // ---------------------------------------------------
            // 3️⃣ Update Lease
            // ---------------------------------------------------
            lease.StartDate = request.NewStartDate;
            lease.EndDate = request.NewEndDate;
            lease.Rent = request.NewRent;

            if (request.NewDeposit.HasValue)
                lease.Deposit = request.NewDeposit.Value;

            lease.UpdatedAt = DateTime.UtcNow;

            _db.Leases.Update(lease);

            // ---------------------------------------------------
            // 4️ Close Old Rent Schedules
            // ---------------------------------------------------
            var futureSchedules = lease.RentSchedules
                .Where(rs => rs.DueDate >= request.NewStartDate && !rs.IsPaid)
                .ToList();

            foreach (var schedule in futureSchedules)
            {
                schedule.IsDeleted = true;
            }

            // ---------------------------------------------------
            // 5️ Generate New Rent Schedules
            // ---------------------------------------------------
            var newSchedules = GenerateRentSchedule(
                lease,
                request.NewStartDate,
                request.NewEndDate);

            _db.RentSchedules.AddRange(newSchedules);

            // ---------------------------------------------------
            // 6️ Save
            // ---------------------------------------------------
            await _db.SaveChangesAsync(cancellationToken);

            return renewal.LeaseRenewalID;
        }

        // ---------------------------------------------------
        // Rent Schedule Generator (reuse-safe)
        // ---------------------------------------------------
        private static IEnumerable<RentSchedule> GenerateRentSchedule(
            Lease lease,
            DateTime startDate,
            DateTime endDate)
        {
            var schedules = new System.Collections.Generic.List<RentSchedule>();
            DateTime dueDate = startDate;

            while (dueDate <= endDate)
            {
                schedules.Add(new RentSchedule
                {
                    LeaseID = lease.LeaseID,
                    DueDate = dueDate,
                    Amount = lease.Rent,
                    IsPaid = false
                });

                dueDate = lease.PaymentFrequency.ToLower() switch
                {
                    "weekly" => dueDate.AddDays(7),
                    "monthly" => dueDate.AddMonths(1),
                    "quarterly" => dueDate.AddMonths(3),
                    "yearly" => dueDate.AddYears(1),
                    _ => dueDate.AddMonths(1)
                };
            }

            return schedules;
        }
    }
}
