using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Addresses.DTOs;
using TPMS.Application.Features.Tenants.DTOs;
using TPMS.Application.Features.Tenants.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Tenants.Handlers
{
    public class GetDeletedTenantsHandler : IRequestHandler<GetDeletedTenantsQuery, IEnumerable<TenantDto>>
    {
        private readonly TPMSDBContext _db;

        public GetDeletedTenantsHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TenantDto>> Handle(GetDeletedTenantsQuery request, CancellationToken cancellationToken)
        {
            // ✅ Get OwnerTypeID for Tenant safely
            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Tenant")
                .Select(o => (int?)o.OwnerTypeID)
                .FirstOrDefaultAsync(cancellationToken);

            if (ownerTypeId == null)
                throw new InvalidOperationException("OwnerType 'Tenant' not found in the database. Please seed it before using this feature.");

            // ✅ Load all deleted tenants
            var deletedTenants = await _db.Tenants
                .AsNoTracking()
                .Where(t => t.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!deletedTenants.Any())
                return Enumerable.Empty<TenantDto>();

            // ✅ Batch-load all addresses for these tenants (avoid N+1)
            var tenantIds = deletedTenants.Select(t => t.TenantID).ToList();

            var addresses = await _db.Addresses
                .Where(a => a.OwnerTypeID == ownerTypeId && tenantIds.Contains(a.OwnerID))
                .ToListAsync(cancellationToken);

            // ✅ Combine results in memory
            var result = deletedTenants.Select(t => new TenantDto
            {
                TenantID = t.TenantID,
                Name = t.Name,
                Notes = t.Notes,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                Addresses = addresses
                    .Where(a => a.OwnerID == t.TenantID && a.IsPrimary)
                    .Select(a => new TenantAddressDto()
                    {
                        AddressLine1 = a.AddressLine1,
                        City = a.City,
                        State = a.State,
                        Country = a.Country,
                        PostalCode = a.PostalCode,
                        Phone1 = a.Phone1,
                        Email = a.Email,
                        IsPrimary = a.IsPrimary
                    })
                    .ToList()
            });

            return result;
        }
    }
}
