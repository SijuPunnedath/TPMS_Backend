using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Common.Interfaces;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Application.Features.Leases.Queries;
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class GetAllLeasesExtendedQueryHandler
         : IRequestHandler<GetAllLeasesExtendedQuery, PagedResult<LeaseDto>>
    {
        private readonly TPMSDBContext _db;
        private readonly IOwnerTypeCacheService _ownerTypeCache;

        public GetAllLeasesExtendedQueryHandler(TPMSDBContext db, IOwnerTypeCacheService ownerTypeCache)
        {
            _db = db;
            _ownerTypeCache = ownerTypeCache;
        }

        public async Task<PagedResult<LeaseDto>> Handle(GetAllLeasesExtendedQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Leases
                .Include(l => l.Tenant)
                .Include(l => l.Landlord)
                .Include(l => l.Property)
                .Where(l => !l.IsDeleted)
                .AsQueryable();

            // 🔹 Apply filters
            if (request.LandlordId.HasValue)
                query = query.Where(l => l.LandlordID == request.LandlordId.Value);

            if (request.TenantId.HasValue)
                query = query.Where(l => l.TenantID == request.TenantId.Value);

            if (request.PropertyId.HasValue)
                query = query.Where(l => l.PropertyID == request.PropertyId.Value);
            

            if (request.Status.HasValue)
                query = query.Where(l => l.Status == request.Status.Value);
            
            if (request.FromDate.HasValue)
                query = query.Where(l => l.StartDate >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(l => l.EndDate <= request.ToDate.Value);

            //  Keyword search
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                string keyword = request.SearchTerm.Trim().ToLower();

                int propertyTypeId = _ownerTypeCache.GetOwnerTypeId("Property");
                int tenantTypeId = _ownerTypeCache.GetOwnerTypeId("Tenant");
                int landlordTypeId = _ownerTypeCache.GetOwnerTypeId("Landlord");

                // Search in address table
                var propertyIdsWithMatchingAddress = _db.Addresses
                    .Where(a => a.OwnerTypeID == propertyTypeId &&
                        (a.AddressLine1.ToLower().Contains(keyword) ||
                         a.AddressLine2.ToLower().Contains(keyword) ||
                         a.City.ToLower().Contains(keyword) ||
                         a.State.ToLower().Contains(keyword) ||
                         a.Country.ToLower().Contains(keyword) ||
                         a.PostalCode.ToLower().Contains(keyword)))
                    .Select(a => a.OwnerID);

                // Combine keyword search conditions
                query = query.Where(l =>
                    (l.Tenant != null && l.Tenant.Name.ToLower().Contains(keyword)) ||
                    (l.Landlord != null && l.Landlord.Name.ToLower().Contains(keyword)) ||
                    (l.Property != null && l.Property.Type.ToLower().Contains(keyword)) ||
                    propertyIdsWithMatchingAddress.Contains(l.PropertyID));
            }

            ////  Sorting
            //var sortBy = request.SortBy ?? "CreatedAt";
            //var sortDirection = request.SortDirection?.ToLower() == "asc" ? "asc" : "desc";
            //query = query.OrderBy($"{sortBy} {sortDirection}");

            //  Sorting
            //var sortBy = request.SortBy ?? "CreatedAt";
            //var sortDirection = request.SortDirection?.ToLower() == "asc" ? "asc" : "desc";
            //query = query.OrderBy($"{sortBy} {sortDirection}");

            switch (request.SortBy?.ToLower())
            {
                case "startdate":
                    query = request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(l => l.StartDate)
                        : query.OrderByDescending(l => l.StartDate);
                    break;
                case "enddate":
                    query = request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(l => l.EndDate)
                        : query.OrderByDescending(l => l.EndDate);
                    break;
                case "rent":
                    query = request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(l => l.Rent)
                        : query.OrderByDescending(l => l.Rent);
                    break;
                default:
                    query = request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(l => l.CreatedAt)
                        : query.OrderByDescending(l => l.CreatedAt);
                    break;
            }

            //  Pagination
            var totalRecords = await query.CountAsync(cancellationToken);

            var leases = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            //  Map to DTO
            var result = leases.Select(l => new LeaseDto
            {
                LeaseID = l.LeaseID,
                LeaseNumber = l.LeaseNumber,
                PropertyID = l.PropertyID,
                PropertyNumber = l.Property != null ? l.Property.PropertyNumber : "",
                TenantID = l.TenantID,
                LandlordID = l.LandlordID,
                StartDate = l.StartDate,
                DateMovedIn = l.DateMovedIn,
                EndDate = l.EndDate,
                Rent = l.Rent,
                Deposit = l.Deposit,
                Status = l.Status,
                PaymentFrequency = l.PaymentFrequency,
                PenaltyPolicyID = l.PenaltyPolicyID,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            }).ToList();

            // 🔹 Wrap in PagedResult
            return new PagedResult<LeaseDto>(result, totalRecords, request.PageNumber, request.PageSize);
        }
    }
}

