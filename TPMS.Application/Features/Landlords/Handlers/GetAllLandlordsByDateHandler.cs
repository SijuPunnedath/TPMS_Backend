using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Application.Features.Landlords.Queries;
using TPMS.Infrastructure.Common.Behaviours;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Landlords.Handlers
{
    public class GetAllLandlordsByDateHandler : IRequestHandler<GetAllLandlordsByDateQuery, PagedResult<LandlordDto>>
    {
        private readonly TPMSDBContext _db;

        public GetAllLandlordsByDateHandler(TPMSDBContext db) => _db = db;

        public async Task<PagedResult<LandlordDto>> Handle(GetAllLandlordsByDateQuery request, CancellationToken cancellationToken)
        {
            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Landlord")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            var query = _db.Landlords.AsNoTracking();

            // 🔹 Search filter
            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(l =>
                    (l.Name ?? "").ToLower().Contains(search) ||
                    (l.Notes ?? "").ToLower().Contains(search));
            }

            // 🔹 Date range filter
            if (request.StartDate.HasValue)
                query = query.Where(l => l.CreatedAt >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(l => l.CreatedAt <= request.EndDate.Value);

            // 🔹 Sorting
            query = request.SortBy?.ToLower() switch
            {
                "name" => request.SortOrder == "desc"
                    ? query.OrderByDescending(l => l.Name)
                    : query.OrderBy(l => l.Name),

                "createdat" => request.SortOrder == "desc"
                    ? query.OrderByDescending(l => l.CreatedAt)
                    : query.OrderBy(l => l.CreatedAt),

                "updatedat" => request.SortOrder == "desc"
                    ? query.OrderByDescending(l => l.UpdatedAt)
                    : query.OrderBy(l => l.UpdatedAt),

                _ => query.OrderBy(l => l.LandlordID)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var landlords = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(l => new LandlordDto
                {
                    LandlordID = l.LandlordID,
                    LandlordNumber = l.LandlordNumber,
                    Name = l.Name ?? string.Empty,
                    Notes = l.Notes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,
                    LandlordAddress = _db.Addresses
                        .Where(a => a.OwnerTypeID == ownerTypeId && a.OwnerID == l.LandlordID && a.IsPrimary)
                        .Select(a => new LandlordAddressDto
                        {
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
                        .FirstOrDefault() ?? new LandlordAddressDto()
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<LandlordDto>
            {
                Items = landlords,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}

