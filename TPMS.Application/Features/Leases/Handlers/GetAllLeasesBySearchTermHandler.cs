using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Features.Addresses.DTOs;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Application.Features.Leases.Queries;
using TPMS.Application.Features.RentSchedules.DTOs;
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class GetAllLeasesBySearchTermHandler
        : IRequestHandler<GetAllLeasesBySearchTermQuery, List<LeaseWithSearchTermDto>>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;

        public GetAllLeasesBySearchTermHandler(
            TPMSDBContext db,
            IOwnerTypeCacheService ownerTypeCache)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
        }

        public async Task<List<LeaseWithSearchTermDto>> Handle(
            GetAllLeasesBySearchTermQuery request,
            CancellationToken cancellationToken)
        {
            // ----------------------------
            // Base query
            // ----------------------------
            var query = _db.Leases
                .Include(l => l.Tenant)
                .Include(l => l.Landlord)
                .Include(l => l.Property)
                .Include(l => l.RentSchedules)
                .Where(l => !l.IsDeleted)
                .AsQueryable();

            int propertyTypeId = _ownerTypeCache.GetOwnerTypeId("Property");
            int tenantTypeId = _ownerTypeCache.GetOwnerTypeId("Tenant");
            int landlordTypeId = _ownerTypeCache.GetOwnerTypeId("Landlord");

            // ----------------------------
            // Search filter
            // ----------------------------
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var keyword = request.SearchTerm.Trim().ToLower();

                var propertyIds = _db.Addresses
                    .Where(a => a.OwnerTypeID == propertyTypeId &&
                        (a.AddressLine1.ToLower().Contains(keyword) ||
                         a.City.ToLower().Contains(keyword)))
                    .Select(a => a.OwnerID);

                var tenantIds = _db.Addresses
                    .Where(a => a.OwnerTypeID == tenantTypeId &&
                        (a.AddressLine1.ToLower().Contains(keyword) ||
                         a.City.ToLower().Contains(keyword)))
                    .Select(a => a.OwnerID);

                var landlordIds = _db.Addresses
                    .Where(a => a.OwnerTypeID == landlordTypeId &&
                        (a.AddressLine1.ToLower().Contains(keyword) ||
                         a.City.ToLower().Contains(keyword)))
                    .Select(a => a.OwnerID);

                query = query.Where(l =>
                    (l.Tenant != null && l.Tenant.Name.ToLower().Contains(keyword)) ||
                    (l.Landlord != null && l.Landlord.Name.ToLower().Contains(keyword)) ||
                    propertyIds.Contains(l.PropertyID) ||
                    (l.TenantID.HasValue && tenantIds.Contains(l.TenantID.Value)) ||
                    (l.LandlordID.HasValue && landlordIds.Contains(l.LandlordID.Value)) ||
                    l.LeaseType.ToString().ToLower().Contains(keyword)
                );
            }

            var leases = await query
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync(cancellationToken);

            // ----------------------------
            // Load primary addresses
            // ----------------------------
            var addresses = await _db.Addresses
                .Where(a => a.IsPrimary)
                .ToListAsync(cancellationToken);

            var propertyAddresses = addresses
                .Where(a => a.OwnerTypeID == propertyTypeId)
                .ToDictionary(a => a.OwnerID);

            var tenantAddresses = addresses
                .Where(a => a.OwnerTypeID == tenantTypeId)
                .ToDictionary(a => a.OwnerID);

            var landlordAddresses = addresses
                .Where(a => a.OwnerTypeID == landlordTypeId)
                .ToDictionary(a => a.OwnerID);

            // ----------------------------
            // Map to DTOs
            // ----------------------------
            return leases.Select(l =>
            {
                propertyAddresses.TryGetValue(l.PropertyID, out Address? pAddr);

                AddressDto? tenantAddress = null;
                if (l.TenantID.HasValue &&
                    tenantAddresses.TryGetValue(l.TenantID.Value, out var tAddr))
                {
                    tenantAddress = MapAddress(tAddr);
                }

                AddressDto? landlordAddress = null;
                if (l.LandlordID.HasValue &&
                    landlordAddresses.TryGetValue(l.LandlordID.Value, out var ldAddr))
                {
                    landlordAddress = MapAddress(ldAddr);
                }

                return new LeaseWithSearchTermDto
                {
                    LeaseID = l.LeaseID,
                    LeaseNumber = l.LeaseNumber,
                    LeaseName = l.LeaseName,
                    PropertyID = l.PropertyID,
                    PropertyNumber = l.Property?.PropertyNumber ?? "",
                    TenantID = l.TenantID,
                    LandlordID = l.LandlordID,

                    LeaseType = l.LeaseType,
                    LeaseNotes = l.LeaseNotes,
                    Commission = l.Commission,

                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    DateMovedIn = l.DateMovedIn,
                    Rent = l.Rent,
                    Deposit = l.Deposit,
                    Status = l.Status,
                    PaymentFrequency = l.PaymentFrequency,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,

                    TenantName = l.Tenant?.Name ?? "",
                    LandlordName = l.Landlord?.Name ?? "",
                    PropertyName = l.Property?.Type ?? "",

                    PropertyAddress = pAddr == null ? null : MapAddress(pAddr),
                    TenantAddress = tenantAddress,
                    LandlordAddress = landlordAddress,

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

        // ----------------------------
        // Mapper
        // ----------------------------
        private static AddressDto MapAddress(Address address)
        {
            return new AddressDto
            {
                AddressID = address.AddressID,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                Phone1 = address.Phone1,
                Phone2 = address.Phone2,
                Email = address.Email
            };
        }
    }
}
