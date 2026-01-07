using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Application.Features.Leases.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class GetLeaseByIdHandler : IRequestHandler<GetLeaseByIdQuery, LeaseDto?>
    {
        private readonly TPMSDBContext _db;
        private readonly IMapper _mapper;
        public GetLeaseByIdHandler(TPMSDBContext db,IMapper mapper)
        {
          _db = db;
            _mapper = mapper;
        }

        public async Task<LeaseDto?> Handle(GetLeaseByIdQuery request, CancellationToken cancellationToken)
        {
            var lease = await _db.Leases.AsNoTracking()
                .FirstOrDefaultAsync(l => l.LeaseID == request.LeaseId && !l.IsDeleted, cancellationToken);

            if (lease == null) return null;

            //--  AutoMapper handles nested lists (RentSchedules, LeaseAlerts)
            return _mapper.Map<LeaseDto>(lease);

            //return new LeaseDto
            //{
            //    LeaseID = lease.LeaseID,
            //    PropertyID = lease.PropertyID,
            //    TenantID = lease.TenantID,
            //    LandlordID = lease.LandlordID,
            //    StartDate = lease.StartDate,
            //    EndDate = lease.EndDate,
            //    Rent = lease.Rent,
            //    Deposit = lease.Deposit,
            //    Status = lease.Status,
            //    PaymentFrequency = lease.PaymentFrequency,
            //    PenaltyPolicyID = lease.PenaltyPolicyID,
            //    CreatedAt = lease.CreatedAt,
            //    UpdatedAt = lease.UpdatedAt
            //};
        }
    }
}
