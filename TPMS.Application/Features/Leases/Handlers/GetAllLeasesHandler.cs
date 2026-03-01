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
    public class GetAllLeasesHandler:IRequestHandler<GetAllLeasesQuery, List<LeaseDto>>
    {
        private readonly TPMSDBContext _db;
        private readonly IMapper _mapper;

        public GetAllLeasesHandler(TPMSDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<LeaseDto>> Handle(GetAllLeasesQuery request, CancellationToken cancellationToken)
        {
           
                var query = _db.Leases
                    .Include(l => l.Tenant)
                    .Include(l => l.Landlord)
                    .Include(l => l.RentSchedules)
                    .Include(l => l.LeaseAlerts)
                    .Include(l => l.Property)
                    .AsNoTracking()
                    .Where(l => !l.IsDeleted);

            // Optional filters
            if (request.LandlordId.HasValue)
                query = query.Where(l => l.LandlordID == request.LandlordId);

            if (request.TenantId.HasValue)
                query = query.Where(l => l.TenantID == request.TenantId);

            if (request.PropertyId.HasValue)
                query = query.Where(l => l.PropertyID == request.PropertyId);
            
            if (request.Status.HasValue)
            {
                query = query.Where(l => l.Status == request.Status.Value);
            }
           
            if (request.FromDate.HasValue)
                query = query.Where(l => l.StartDate >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(l => l.EndDate <= request.ToDate.Value);

            var leases = await query
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync(cancellationToken);

            //  Use AutoMapper to map entity → DTO (including nested collections)
            var leaseDtos = _mapper.Map<List<LeaseDto>>(leases);

            
            return leaseDtos;
        }

    }
}
