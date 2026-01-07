using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Application.Features.Leases.Queries;
using TPMS.Application.Features.Addresses.DTOs;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.RentSchedules.DTOs;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class GetLeasesByTypeHandler 
        : IRequestHandler<GetLeasesByTypeQuery, List<LeaseWithSearchTermDto>>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;

        public GetLeasesByTypeHandler(
            TPMSDBContext db,
            IOwnerTypeCacheService ownerTypeCache)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
        }

        public async Task<List<LeaseWithSearchTermDto>> Handle(
            GetLeasesByTypeQuery request,
            CancellationToken cancellationToken)
        {
            // -----------------------------
            // BASE QUERY
            // -----------------------------
            var query = _db.Leases
                .Include(l => l.Property)
                .Include(l => l.Tenant)
                .Include(l => l.Landlord)
                .Include(l => l.RentSchedules)
                .Where(l => !l.IsDeleted && l.LeaseType == request.LeaseType)
                .AsQueryable();

            // -----------------------------
            // SEARCH
            // -----------------------------
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                string keyword = request.SearchTerm.Trim().ToLower();

                int propertyTypeId = _ownerTypeCache.GetOwnerTypeId("Property");
                int tenantTypeId = _ownerTypeCache.GetOwnerTypeId("Tenant");
                int landlordTypeId = _ownerTypeCache.GetOwnerTypeId("Landlord");

                var propertyMatches = _db.Addresses
                    .Where(a =>
                        a.OwnerTypeID == propertyTypeId &&
                        (a.AddressLine1.ToLower().Contains(keyword) ||
                         a.City.ToLower().Contains(keyword)))
                    .Select(a => a.OwnerID);

                var tenantMatches = _db.Addresses
                    .Where(a =>
                        a.OwnerTypeID == tenantTypeId &&
                        (a.AddressLine1.ToLower().Contains(keyword) ||
                         a.City.ToLower().Contains(keyword)))
                    .Select(a => a.OwnerID);

                var landlordMatches = _db.Addresses
                    .Where(a =>
                        a.OwnerTypeID == landlordTypeId &&
                        (a.AddressLine1.ToLower().Contains(keyword) ||
                         a.City.ToLower().Contains(keyword)))
                    .Select(a => a.OwnerID);

                query = query.Where(l =>
                    (l.Tenant != null && l.Tenant.Name.ToLower().Contains(keyword)) ||
                    (l.Landlord != null && l.Landlord.Name.ToLower().Contains(keyword)) ||
                    propertyMatches.Contains(l.PropertyID) ||
                    (l.TenantID.HasValue && tenantMatches.Contains(l.TenantID.Value)) ||
                    (l.LandlordID.HasValue && landlordMatches.Contains(l.LandlordID.Value))
                );
            }

            // -----------------------------
            // EXECUTE QUERY
            // -----------------------------
            var leases = await query
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync(cancellationToken);

            // -----------------------------
            // LOAD PRIMARY ADDRESSES
            // -----------------------------
            var addresses = await _db.Addresses
                .Where(a => a.IsPrimary)
                .ToListAsync(cancellationToken);

            int propertyType = _ownerTypeCache.GetOwnerTypeId("Property");
            int tenantType = _ownerTypeCache.GetOwnerTypeId("Tenant");
            int landlordType = _ownerTypeCache.GetOwnerTypeId("Landlord");

            var propertyAddressMap = addresses
                .Where(a => a.OwnerTypeID == propertyType)
                .ToDictionary(a => a.OwnerID);

            var tenantAddressMap = addresses
                .Where(a => a.OwnerTypeID == tenantType)
                .ToDictionary(a => a.OwnerID);

            var landlordAddressMap = addresses
                .Where(a => a.OwnerTypeID == landlordType)
                .ToDictionary(a => a.OwnerID);

            // -----------------------------
            // MAP RESULT
            // -----------------------------
            return leases.Select(l =>
            {
                propertyAddressMap.TryGetValue(l.PropertyID, out var pAddr);

                AddressDto? tenantAddr = null;
                if (l.TenantID.HasValue &&
                    tenantAddressMap.TryGetValue(l.TenantID.Value, out var tAddr))
                {
                    tenantAddr = MapAddress(tAddr);
                }

                AddressDto? landlordAddr = null;
                if (l.LandlordID.HasValue &&
                    landlordAddressMap.TryGetValue(l.LandlordID.Value, out var ldAddr))
                {
                    landlordAddr = MapAddress(ldAddr);
                }

                return new LeaseWithSearchTermDto
                {
                    LeaseID = l.LeaseID,
                    PropertyID = l.PropertyID,
                    TenantID = l.TenantID,
                    LandlordID = l.LandlordID,

                    LeaseType = l.LeaseType,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    DateMovedIn = l.DateMovedIn,
                    Rent = l.Rent,
                    Deposit = l.Deposit,
                    Status = l.Status,
                    PaymentFrequency = l.PaymentFrequency,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,

                    TenantName = l.Tenant?.Name ?? string.Empty,
                    LandlordName = l.Landlord?.Name ?? string.Empty,
                    PropertyName = l.Property?.Type ?? string.Empty,

                    PropertyAddress = pAddr == null ? null : MapAddress(pAddr),
                    TenantAddress = tenantAddr,
                    LandlordAddress = landlordAddr,

                    RentSchedules = l.RentSchedules.Select(rs => new RentScheduleDto
                    {
                        ScheduleID = rs.ScheduleID,
                        DueDate = rs.DueDate,
                        Amount = rs.Amount,
                        IsPaid = rs.IsPaid,
                        PaidDate = rs.PaidDate,
                        Penalty = rs.Penalty
                    }).ToList()
                };
            }).ToList();
        }

        // -----------------------------
        // ADDRESS MAPPER (IMPORTANT)
        // -----------------------------
        private static AddressDto MapAddress(TPMS.Domain.Entities.Address a)
        {
            return new AddressDto
            {
                AddressID = a.AddressID,
                AddressLine1 = a.AddressLine1,
                City = a.City,
                State = a.State,
                Country = a.Country,
                PostalCode = a.PostalCode
            };
        }
    }
}
