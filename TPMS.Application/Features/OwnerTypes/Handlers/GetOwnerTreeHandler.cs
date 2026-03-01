using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.OwnerTypes.DTOs;
using TPMS.Application.Features.OwnerTypes.Queries;
using TPMS.Application.Features.OwnerTypes.DTOs;
using TPMS.Application.Features.OwnerTypes.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.OwnerTypes.Handlers
{
    public class GetOwnerTreeHandler 
        : IRequestHandler<GetOwnerTreeQuery, List<OwnerTreeDto>>
    {
        private readonly TPMSDBContext _db;

        public GetOwnerTreeHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<List<OwnerTreeDto>> Handle(
            GetOwnerTreeQuery request,
            CancellationToken cancellationToken)
        {
            var ownerTypes = await _db.OwnerTypes
                .Where(o => o.IsActive && !o.IsDeleted)
                .OrderBy(o => o.Name)
                .ToListAsync(cancellationToken);

            var result = new List<OwnerTreeDto>();

            foreach (var ownerType in ownerTypes)
            {
                var owners = await GetOwnersByType(
                    ownerType.Name!,
                    cancellationToken);

                result.Add(new OwnerTreeDto
                {
                    OwnerTypeID = ownerType.OwnerTypeID,
                    OwnerTypeName = ownerType.Name!,
                    Owners = owners
                });
            }

            return result;
        }

        private async Task<List<OwnerNodeDto>> GetOwnersByType(
            string ownerTypeName,
            CancellationToken cancellationToken)
        {
            return ownerTypeName switch
            {
                "Landlord" => await _db.Landlords
                    .Where(l =>  !l.IsDeleted)
                    .Select(l => new OwnerNodeDto
                    {
                        OwnerID = l.LandlordID,
                        OwnerName = l.Name
                    })
                    .ToListAsync(cancellationToken),

                "Tenant" => await _db.Tenants
                    .Where(t =>  !t.IsDeleted)
                    .Select(t => new OwnerNodeDto
                    {
                        OwnerID = t.TenantID,
                        OwnerName = t.Name
                    })
                    .ToListAsync(cancellationToken),

                "Lease" => await _db.Leases
                    .Where(l => !l.IsDeleted)
                    .Select(l => new OwnerNodeDto
                    {
                        OwnerID = l.LeaseID,
                        OwnerName = l.LeaseNumber,//l.LeaseName
                    })
                    .ToListAsync(cancellationToken),

                "Property" => await _db.Properties
                    .Where(p => !p.IsDeleted)
                    .Select(p => new OwnerNodeDto
                    {
                        OwnerID = p.PropertyID,
                        OwnerName = p.PropertyName
                    })
                    .ToListAsync(cancellationToken),

                //  NEW: General → Companies
                "General" => await _db.CompanySettings
                    .Where(c => !c.IsDeleted)
                    .Select(c => new OwnerNodeDto
                    {
                        OwnerID = c.CompanyID,
                        OwnerName = c.CompanyName
                    })
                    .ToListAsync(cancellationToken),
                
                _ => new List<OwnerNodeDto>()
            };
        }
    }
}
