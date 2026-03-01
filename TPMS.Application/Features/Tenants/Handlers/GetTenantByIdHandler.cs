using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Addresses.DTOs;
using TPMS.Application.Features.Tenants.DTOs;
using TPMS.Application.Features.Tenants.Queries;
using TPMS.Infrastructure.Persistence.Configurations;
//using TPMS.Application.Interfaces.Caching;

namespace TPMS.Application.Features.Tenants.Handlers
{
    public class GetTenantByIdHandler : IRequestHandler<GetTenantByIdQuery, TenantDto?>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;

        public GetTenantByIdHandler(TPMSDBContext db, IOwnerTypeCacheService ownerTypeCache)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
        }

        public async Task<TenantDto?> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
        {
            //  Fetch tenant
            var tenant = await _db.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TenantID == request.TenantId && !t.IsDeleted, cancellationToken);

            if (tenant == null)
                return null;

            //  Get OwnerTypeID for "Tenant"
            int tenantTypeId = _ownerTypeCache.GetOwnerTypeId("Tenant");

            //  Fetch all addresses for this tenant
            var addresses = await _db.Addresses
                .Where(a => a.OwnerTypeID == tenantTypeId && a.OwnerID == tenant.TenantID)
                .ToListAsync(cancellationToken);

            //  Map to DTO
            var tenantDto = new TenantDto
            {
                TenantID = tenant.TenantID,
                TenantNumber = tenant.TenantNumber,
                Name = tenant.Name ?? string.Empty,
                Notes = tenant.Notes,
                CreatedAt = tenant.CreatedAt,
                UpdatedAt = tenant.UpdatedAt,
                Addresses = addresses.Select(a => new TenantAddressDto
                {
                    AddressID = a.AddressID,
                    AddressLine1 = a.AddressLine1,
                    AddressLine2 = a.AddressLine2,
                    City = a.City,
                    State = a.State,
                    Country = a.Country,
                    PostalCode = a.PostalCode,
                    Phone1 = a.Phone1,
                    Phone2 = a.Phone2,
                    Email = a.Email,
                    IsPrimary = a.IsPrimary
                }).ToList()
            };

            return tenantDto;
        }
    }
}
