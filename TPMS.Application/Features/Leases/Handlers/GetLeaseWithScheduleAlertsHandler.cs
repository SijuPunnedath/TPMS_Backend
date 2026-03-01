using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.LeaseAlert.DTOs;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Application.Features.Leases.Queries;
using TPMS.Application.Features.RentSchedules.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class GetLeaseWithScheduleAlertsHandler : IRequestHandler<GetLeaseWithScheduleAlertsByIdQuery, LeaseWithScheduleAlertsDto>
    {
        private readonly TPMSDBContext _db;
        public GetLeaseWithScheduleAlertsHandler(TPMSDBContext db) => _db = db;

        public async Task<LeaseWithScheduleAlertsDto> Handle(GetLeaseWithScheduleAlertsByIdQuery request, CancellationToken cancellationToken)
        {
            var lease = await _db.Leases
                .Include(l => l.RentSchedules)
                .Include(l => l.LeaseAlerts)
                .Include(l => l.Property)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.LeaseID == request.LeaseId && !l.IsDeleted, cancellationToken);

            if (lease == null)
            {
                throw new InvalidOperationException($"Lease with ID {request.LeaseId} not found.");
            }

            return new LeaseWithScheduleAlertsDto
            {
                LeaseID = lease.LeaseID,
                LeaseNumber = lease.LeaseNumber,
                PropertyID = lease.PropertyID,
                PropertyNumber = lease.Property !=null? lease.Property.PropertyNumber: "",
                TenantID = lease.TenantID,
                LandlordID = lease.LandlordID,
                StartDate = lease.StartDate,
                EndDate = lease.EndDate,
                DateMovedIn = lease.DateMovedIn,
                Rent = lease.Rent,
                Deposit = lease.Deposit,
                Status = lease.Status,
                PaymentFrequency = lease.PaymentFrequency,
                PenaltyPolicyID = lease.PenaltyPolicyID,
                CreatedAt = lease.CreatedAt,
                UpdatedAt = lease.UpdatedAt,

                RentSchedules = lease.RentSchedules
                    .OrderBy(rs => rs.DueDate)
                    .Select(rs => new RentScheduleDto
                    {
                        ScheduleID = rs.ScheduleID,
                        DueDate = rs.DueDate,
                        Amount = rs.Amount,
                        IsPaid = rs.IsPaid,
                        PaidDate = rs.PaidDate,
                        Penalty = rs.Penalty
                    }).ToList(),

                LeaseAlerts = lease.LeaseAlerts
                    .OrderByDescending(a => a.AlertDate)
                    .Select(a => new LeaseAlertDto
                    {
                        AlertID = a.AlertID,
                        LeaseID = a.LeaseID,
                        Message = a.Message,
                        AlertDate = a.AlertDate,
                        Status = a.Status,
                    }).ToList()
            };
        }
    }
}
