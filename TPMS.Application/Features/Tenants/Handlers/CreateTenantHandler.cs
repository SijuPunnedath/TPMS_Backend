using DocumentFormat.OpenXml.Office2016.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Lookups.Services;
using TPMS.Application.Features.Tenants.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Tenants.Handlers
{
    public class CreateTenantHandler : IRequestHandler<CreateTenantCommand, int>
    {
        private readonly TPMSDBContext _db;
        private readonly ILookupCacheService _cache;
        public CreateTenantHandler(TPMSDBContext db,ILookupCacheService cache)
        {
            _db = db; 
            _cache = cache;
        } 

        public async Task<int> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = new Tenant
            {
                Name = request.Tenant.Name,
                Notes = request.Tenant.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Tenants.Add(tenant);
            await _db.SaveChangesAsync(cancellationToken);

            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Tenant")
                .Select(o => (int?)o.OwnerTypeID)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (ownerTypeId == null)
            {
                throw new InvalidOperationException("OwnerType 'Tenant' not found. Please ensure it's defined in the OwnerTypes table.");
            }

            var address = new Address
            {
                OwnerTypeID = ownerTypeId.Value,
                OwnerID = tenant.TenantID,
                AddressLine1 = request.Tenant.Address.AddressLine1,
                AddressLine2 = request.Tenant.Address.AddressLine2,
                City = request.Tenant.Address.City,
                State = request.Tenant.Address.State,
                Country = request.Tenant.Address.Country,
                PostalCode = request.Tenant.Address.PostalCode,
                Phone1 = request.Tenant.Address.Phone1,
                Phone2 = request.Tenant.Address.Phone2,
                Email = request.Tenant.Address.Email,
                IsPrimary = true
            };

            _db.Addresses.Add(address);
            await _db.SaveChangesAsync(cancellationToken);
            await _cache.RefreshTenantsAsync();
            return tenant.TenantID;
        }
        }
    }

