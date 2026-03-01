using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Leases.Commands;
using TPMS.Domain.Entities;
using TPMS.Domain.Guards;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class CreateLeaseHandler : IRequestHandler<CreateLeaseCommand, int>
    {
        private readonly TPMSDBContext _db;
        private readonly IMapper _mapper;

        public CreateLeaseHandler(TPMSDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateLeaseCommand request, CancellationToken cancellationToken)
        {
            try
            { 
               await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

                var lease = _mapper.Map<Lease>(request.Lease);

                var property = await _db.Properties
                .FirstOrDefaultAsync(p => p.PropertyID == request.Lease.PropertyID, cancellationToken)
                ?? throw new InvalidOperationException("Property not found.");

            //  Domain validation (cleaner)
            EnsureNoActiveLease(property, lease.LeaseType);

            lease.CreatedAt = DateTime.UtcNow;
            lease.UpdatedAt = DateTime.UtcNow;
            lease.Status = LeaseStatus.Active;

            //  Enforce domain rules
            LeaseGuard.Validate(lease);

            _db.Leases.Add(lease);
            await _db.SaveChangesAsync(cancellationToken); // LeaseID generated here

            //  Update property active lease pointer
            SetActiveLeasePointer(property, lease);

            //  Deposit logic
            if (lease.Deposit > 0)
            {
                _db.DepositMasters.Add(new DepositMaster
                {
                    LeaseID = lease.LeaseID,
                    ExpectedAmount = lease.Deposit,
                    PaidAmount = 0,
                    BalanceAmount = lease.Deposit,
                    Status = "Pending",
                    Notes = lease.LeaseType == LeaseType.Inbound
                        ? "Company deposit to landlord"
                        : "Tenant deposit to company"
                });
            }

            //  Generate rent schedules
            var schedules = GenerateRentSchedule(lease);
            _db.RentSchedules.AddRange(schedules);

            await _db.SaveChangesAsync(cancellationToken);
           
            await transaction.CommitAsync(cancellationToken);

            return lease.LeaseID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // ==============================
        // Helper Methods
        // ==============================

        private static void EnsureNoActiveLease(Property property, LeaseType leaseType)
        {
            var hasActiveLease = leaseType switch
            {
                LeaseType.Inbound => property.ActiveInboundLeaseId != null,
                LeaseType.Outbound => property.ActiveOutboundLeaseId != null,
                _ => false
            };

            if (hasActiveLease)
                throw new InvalidOperationException(
                    $"Property already has active {leaseType} lease.");
        }

      /*  private static void SetActiveLeasePointer(Property property, Lease lease)
        {
            if (lease.LeaseType == LeaseType.Inbound)
                property.ActiveInboundLeaseId = lease.LeaseID;
            else
                property.ActiveOutboundLeaseId = lease.LeaseID;
        }*/
      
      private void SetActiveLeasePointer(Property property, Lease lease)
      {
          
          if (lease.LeaseType == LeaseType.Inbound)
          {
              property.ActiveInboundLeaseId = lease.LeaseID;
              property.ActiveOutboundLeaseId ??= property.ActiveOutboundLeaseId;
              property.Status = PropertyStatus.Vacant;
          }
          else
          {
              property.ActiveOutboundLeaseId = lease.LeaseID;
              property.ActiveInboundLeaseId ??= property.ActiveInboundLeaseId;
              property.Status = PropertyStatus.Occupied;
          }

          _db.Entry(property).State = EntityState.Modified;
      }

        private List<RentSchedule> GenerateRentSchedule(Lease lease)
        {
            var schedules = new List<RentSchedule>();
            DateTime dueDate = lease.StartDate;

            while (dueDate <= lease.EndDate)
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
