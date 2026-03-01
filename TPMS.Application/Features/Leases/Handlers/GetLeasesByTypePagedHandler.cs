using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Addresses.DTOs;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Application.Features.Leases.Queries;
using TPMS.Application.Features.RentSchedules.DTOs;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class GetLeasesByTypePagedHandler :
        IRequestHandler<GetLeasesByTypePagedQuery, PagedResult<LeaseWithSearchTermDto>>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;

        public GetLeasesByTypePagedHandler(
            TPMSDBContext db,
            IOwnerTypeCacheService ownerTypeCache)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
        }

        public async Task<PagedResult<LeaseWithSearchTermDto>> Handle(
            GetLeasesByTypePagedQuery request,
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
            // PAGINATION
            // -----------------------------
            int totalCount = await query.CountAsync(cancellationToken);

            var leases = await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // -----------------------------
            // LOAD PRIMARY ADDRESSES
            // -----------------------------
            var addresses = await _db.Addresses
                .Where(a => a.IsPrimary)
                .ToListAsync(cancellationToken);

            int propertyTypeId = _ownerTypeCache.GetOwnerTypeId("Property");
            int tenantTypeId = _ownerTypeCache.GetOwnerTypeId("Tenant");
            int landlordTypeId = _ownerTypeCache.GetOwnerTypeId("Landlord");

            var propertyAddressMap = addresses
                .Where(a => a.OwnerTypeID == propertyTypeId)
                .ToDictionary(a => a.OwnerID);

            var tenantAddressMap = addresses
                .Where(a => a.OwnerTypeID == tenantTypeId)
                .ToDictionary(a => a.OwnerID);

            var landlordAddressMap = addresses
                .Where(a => a.OwnerTypeID == landlordTypeId)
                .ToDictionary(a => a.OwnerID);

            // -----------------------------
            // RESULT MAPPING
            // -----------------------------
            var result = leases.Select(l =>
            {
                // PROPERTY ADDRESS (non-nullable)
                propertyAddressMap.TryGetValue(l.PropertyID, out var pAddr);

                AddressDto? propertyAddress = pAddr == null ? null : MapAddress(pAddr);

                // TENANT ADDRESS (nullable)
                AddressDto? tenantAddress = null;
                if (l.TenantID.HasValue &&
                    tenantAddressMap.TryGetValue(l.TenantID.Value, out var tAddr))
                {
                    tenantAddress = MapAddress(tAddr);
                }

                // LANDLORD ADDRESS (nullable)
                AddressDto? landlordAddress = null;
                if (l.LandlordID.HasValue &&
                    landlordAddressMap.TryGetValue(l.LandlordID.Value, out var ldAddr))
                {
                    landlordAddress = MapAddress(ldAddr);
                }

                return new LeaseWithSearchTermDto
                {
                    LeaseID = l.LeaseID,
                    LeaseNumber = l.LeaseNumber,
                    PropertyID = l.PropertyID,
                    PropertyNumber = l.Property != null ? l.Property.PropertyNumber : "",
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

                    PropertyAddress = propertyAddress,
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

            return new PagedResult<LeaseWithSearchTermDto>(
                result,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }

        // -----------------------------
        // ADDRESS MAPPER (KEY FIX)
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
