using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
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
            var lease = _mapper.Map<Lease>(request.Lease);

            lease.CreatedAt = DateTime.UtcNow;
            lease.UpdatedAt = DateTime.UtcNow;

            // 🔒 Enforce domain rules
            LeaseGuard.Validate(lease);

            _db.Leases.Add(lease);
            await _db.SaveChangesAsync(cancellationToken);

            // Deposit logic (post-save, needs LeaseID)
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

                await _db.SaveChangesAsync(cancellationToken);
            }
            
            // 🔹 Generate Rent Schedule
            var schedules = GenerateRentSchedule(lease);
            _db.RentSchedules.AddRange(schedules);
            await _db.SaveChangesAsync(cancellationToken);

            return lease.LeaseID;
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
                    _ => dueDate.AddMonths(1) // default to monthly
                };
            }

            return schedules;
        }
    }
    
    
}