using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Tenants.DTOs;
using TPMS.Application.Features.Tenants.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Tenants.Handlers;

    public class GetAllTenantsHandler : IRequestHandler<GetAllTenantsQuery, List<TenantDto>>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;

        public GetAllTenantsHandler(TPMSDBContext db, IOwnerTypeCacheService ownerTypeCache)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
        }

        public async Task<List<TenantDto>> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
        {
            int tenantTypeId = _ownerTypeCache.GetOwnerTypeId("Tenant");

            // ✅ Base query
            var tenantsQuery = _db.Tenants
                .AsNoTracking()
                .Where(t => request.IncludeDeleted || !t.IsDeleted)
                .AsQueryable();

            // ✅ Apply search term if provided
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                string keyword = request.SearchTerm.Trim().ToLower();

                // Find tenant IDs matching address or contact info
                var matchingTenantIds = await _db.Addresses
                    .AsNoTracking()
                    .Where(a => a.OwnerTypeID == tenantTypeId &&
                        (
                            (a.AddressLine1 != null && a.AddressLine1.ToLower().Contains(keyword)) ||
                            (a.AddressLine2 != null && a.AddressLine2.ToLower().Contains(keyword)) ||
                            (a.City != null && a.City.ToLower().Contains(keyword)) ||
                            (a.State != null && a.State.ToLower().Contains(keyword)) ||
                            (a.Country != null && a.Country.ToLower().Contains(keyword)) ||
                            (a.Email != null && a.Email.ToLower().Contains(keyword)) ||
                            (a.Phone1 != null && a.Phone1.ToLower().Contains(keyword)) ||
                            (a.Phone2 != null && a.Phone2.ToLower().Contains(keyword))
                        ))
                    .Select(a => a.OwnerID)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                // Apply filters on tenants
                tenantsQuery = tenantsQuery.Where(t =>
                    (t.Name != null && t.Name.ToLower().Contains(keyword)) ||
                    (t.Notes != null && t.Notes.ToLower().Contains(keyword)) ||
                    matchingTenantIds.Contains(t.TenantID));
            }

            // Execute final tenant query
            var tenants = await tenantsQuery
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);

            if (!tenants.Any())
                return new List<TenantDto>();

            // Batch-load all related addresses
            var tenantIds = tenants.Select(t => t.TenantID).ToList();
            var addresses = await _db.Addresses
                .AsNoTracking()
                .Where(a => a.OwnerTypeID == tenantTypeId && tenantIds.Contains(a.OwnerID))
                .ToListAsync(cancellationToken);

            //  Map results
            return tenants.Select(t => new TenantDto
            {
                TenantID = t.TenantID,
                TenantNumber = t.TenantNumber,
                Name = t.Name,
                Notes = t.Notes,
                IsDeleted = t.IsDeleted,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                Addresses = addresses
                    .Where(a => a.OwnerID == t.TenantID)
                    .Select(a => new TenantAddressDto
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
                    })
                    .ToList()
            }).ToList();
        } 
    }
