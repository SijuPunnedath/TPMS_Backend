using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.DocumentSequences.Services;
using TPMS.Application.Features.RenewLease.Commands;
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{ 
    public class RenewLeaseHandler : IRequestHandler<RenewLeaseCommand, int>
   {
    private readonly TPMSDBContext _db;
    private IDocumentNumberService _documentNumberService;

    public RenewLeaseHandler(TPMSDBContext db = null, IDocumentNumberService documentNumberService = null)
    {
        _db = db;
        _documentNumberService = documentNumberService;
    }

    public async Task<int> Handle(
        RenewLeaseCommand request,
        CancellationToken cancellationToken)
    {
        try
        {

       
        using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        // ---------------------------------------------------
        // 1 Load Old Lease
        // ---------------------------------------------------
        var oldLease = await _db.Leases
            .FirstOrDefaultAsync(l => l.LeaseID == request.LeaseID, cancellationToken);

        if (oldLease == null)
            throw new InvalidOperationException("Lease not found.");

        if (oldLease.IsTerminated)
            throw new InvalidOperationException("Terminated lease cannot be renewed.");

        if (oldLease.Status != LeaseStatus.Expired)
            throw new InvalidOperationException("Only expired leases can be renewed.");

        if (request.NewStartDate <= oldLease.EndDate)
            throw new InvalidOperationException("Renewal must start after previous lease end date.");

        // ---------------------------------------------------
        // 2 Load Property
        // ---------------------------------------------------
        var property = await _db.Properties
            .FirstOrDefaultAsync(p => p.PropertyID == oldLease.PropertyID, cancellationToken);

        if (property == null)
            throw new InvalidOperationException("Property not found.");

        // Ensure no active lease exists
        if (oldLease.LeaseType == LeaseType.Inbound &&
            property.ActiveInboundLeaseId != null)
            throw new InvalidOperationException("Property already has active inbound lease.");

        if (oldLease.LeaseType == LeaseType.Outbound &&
            property.ActiveOutboundLeaseId != null)
            throw new InvalidOperationException("Property already has active outbound lease.");

        // ---------------------------------------------------
        // 3 Create New Lease (Versioning Model)
        // ---------------------------------------------------
        var leaseNumber = await _documentNumberService.GenerateAsync("Lease");
        var newLease = new Lease
        {
            PropertyID = oldLease.PropertyID,
            LeaseNumber = leaseNumber,
            TenantID = oldLease.TenantID,
            LeaseType = oldLease.LeaseType,
            StartDate = request.NewStartDate,
            EndDate = request.NewEndDate,
            Rent = request.NewRent,
            Deposit = request.NewDeposit ?? oldLease.Deposit,
            PaymentFrequency = oldLease.PaymentFrequency,
            Status = LeaseStatus.Active,
            ParentLeaseID = oldLease.LeaseID, // optional version tracking
            CreatedAt = DateTime.UtcNow
        };

        _db.Leases.Add(newLease);
        await _db.SaveChangesAsync(cancellationToken);

        // ---------------------------------------------------
        // 4 Update Property Active Lease
        // ---------------------------------------------------
        if (newLease.LeaseType == LeaseType.Inbound)
            property.ActiveInboundLeaseId = newLease.LeaseID;
        else
            property.ActiveOutboundLeaseId = newLease.LeaseID;
       
        // -- Deposit logic
        var oldDeposit = oldLease.Deposit;
        var newDeposit = request.NewDeposit ?? oldDeposit;

        
        HandleDepositRenewal(oldLease, newLease,newDeposit);
        
      /*  
        //1 Mark old lease deposit as transferred
       // oldLease.DepositStatus = DepositStatus.Transferred;

        var difference = newDeposit - oldDeposit;
        
        
        if (newLease.Deposit > 0)
        {
            if (difference == 0)
            {
                _db.DepositMasters.Add(new DepositMaster
                {
                    LeaseID = newLease.LeaseID,
                    ExpectedAmount = newLease.Deposit,
                    PaidAmount = oldLease.Deposit,
                    BalanceAmount = newLease.Deposit,
                    Status = "Paid",
                    Notes = newLease.LeaseType == LeaseType.Inbound
                        ? "Company deposit to landlord"
                        : "Tenant deposit to company"
                });
                
             /*   _db.DepositTransactions.Add(new DepositTransaction
                {
                    LeaseID = newLease.LeaseID,
                    Amount = difference,
                    Type = DepositTransactionType.Additional,
                    TransactionDate 
                }*/
          //  } 
        /*   else if (difference > 0)
       {

         }
         else if (difference < 0)
         {

         }
     } */
        //-- End deposit logic
        
        
        
        // ---------------------------------------------------
        // 5 Generate Rent Schedules
        // ---------------------------------------------------
        var schedules = GenerateRentSchedule(
            newLease,
            request.NewStartDate,
            request.NewEndDate);

        _db.RentSchedules.AddRange(schedules);

        // ---------------------------------------------------
        // 6 Save Everything
        // ---------------------------------------------------
        await _db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return newLease.LeaseID;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void HandleDepositRenewal(
        Lease oldLease,
        Lease newLease,
        decimal? requestedNewDeposit)
    {
        var oldMaster = _db.DepositMasters
            .FirstOrDefault(d => d.LeaseID == oldLease.LeaseID);

        if (oldMaster == null)
            return;

        var oldDeposit = oldMaster.PaidAmount;
        var newDeposit = requestedNewDeposit ?? oldDeposit;
        var difference = newDeposit - oldDeposit;

        // 1 Close old deposit master
        oldMaster.Status = "Transferred";

        // 2 Create new deposit master
        var newMaster = new DepositMaster
        {
            LeaseID = newLease.LeaseID,
            ExpectedAmount = newDeposit,
            PaidAmount = Math.Min(oldDeposit, newDeposit),
            BalanceAmount = difference > 0 ? difference : 0,
            Status = difference > 0 ? "Partially Paid" : "Paid",
            Notes = "Deposit transferred from previous lease"
        };

        _db.DepositMasters.Add(newMaster);
        _db.SaveChanges(); // to get DepositMasterID

        // 3 Handle refund if deposit reduced
        if (difference < 0)
        {
            _db.DepositTransactions.Add(new DepositTransaction
            {
                DepositMasterID = newMaster.DepositMasterID,
                Amount = Math.Abs(difference),
                TransactionDate = DateTime.UtcNow,
                Type = "Refund",
                Notes = "Refund due to reduced deposit on renewal"
            });
        }
    }
    private static IEnumerable<RentSchedule> GenerateRentSchedule(
        Lease lease,
        DateTime startDate,
        DateTime endDate)
    {
        var schedules = new List<RentSchedule>();
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



/*

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
using TPMS.Domain.Enums;
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
            // 1 Load Lease
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

            if (lease.Status != LeaseStatus.Active)
                throw new InvalidOperationException("Only active leases can be renewed.");

            if (request.NewEndDate <= lease.EndDate)
                throw new InvalidOperationException("New end date must be after current end date.");

            // ---------------------------------------------------
            // 2 Create Renewal Record
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
            // 3 Update Lease
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
} */
