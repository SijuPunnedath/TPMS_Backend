using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Properties.DTOs;
using TPMS.Application.Features.Properties.Queries;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Properties.Handlers
{
    public class GetAllPropertiesExtendedHandler
        : IRequestHandler<GetAllPropertiesExtendedQuery, IEnumerable<PropertyDto>>
    {
        private readonly TPMSDBContext _db;

        public GetAllPropertiesExtendedHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<PropertyDto>> Handle(
            GetAllPropertiesExtendedQuery request,
            CancellationToken cancellationToken)
        {
            // 1 Resolve OwnerTypeID
            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Property")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            // 2 Base query
            IQueryable<Property> query = _db.Properties
                .AsNoTracking()
                .Where(p => !p.IsDeleted);

            //  Filtering
            if (!string.IsNullOrWhiteSpace(request.Type))
                query = query.Where(p => p.Type == request.Type);

            if (request.LandlordId.HasValue)
                query = query.Where(p => p.LandlordID == request.LandlordId);
            

            if (!string.IsNullOrWhiteSpace(request.City))
            {
                var city = request.City.ToLower();
                query = query.Where(p =>
                    _db.Addresses.Any(a =>
                        a.OwnerTypeID == ownerTypeId &&
                        a.OwnerID == p.PropertyID &&
                        a.City.ToLower().Contains(city)));
            }

            //  Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(p =>
                    (p.SerialNo != null && p.SerialNo.ToLower().Contains(search)) ||
                    (p.Notes != null && p.Notes.ToLower().Contains(search)));
            }

            //  Sorting
            query = request.SortBy?.ToLower() switch
            {
                "serialno" => request.SortDesc
                    ? query.OrderByDescending(p => p.SerialNo)
                    : query.OrderBy(p => p.SerialNo),

                "type" => request.SortDesc
                    ? query.OrderByDescending(p => p.Type)
                    : query.OrderBy(p => p.Type),

                "size" => request.SortDesc
                    ? query.OrderByDescending(p => p.Size)
                    : query.OrderBy(p => p.Size),

                "createdat" => request.SortDesc
                    ? query.OrderByDescending(p => p.CreatedAt)
                    : query.OrderBy(p => p.CreatedAt),

                _ => query.OrderBy(p => p.PropertyID)
            };

            //  Pagination
            query = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            // 3 Explicit LEFT JOIN + Projection
            var properties = await (
                from p in query

                join l in _db.Landlords.AsNoTracking()
                    on p.LandlordID equals l.LandlordID into landlordGroup
                from landlord in landlordGroup.DefaultIfEmpty()

                select new PropertyDto
                {
                    PropertyID = p.PropertyID,
                    PropertyName = p.PropertyName,
                    PropertyNumber = p.PropertyNumber,
                    SerialNo = p.SerialNo,
                    Type = p.Type,
                    Size = p.Size,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,

                    LandlordID = p.LandlordID,
                    LandlordName = landlord != null ? landlord.Name : null,

                    Address = _db.Addresses
                        .Where(a =>
                            a.OwnerTypeID == ownerTypeId &&
                            a.OwnerID == p.PropertyID &&
                            a.IsPrimary)
                        .Select(a => new PropertyAddressDto
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
                        .FirstOrDefault() ?? new PropertyAddressDto()
                })
                .ToListAsync(cancellationToken);

            return properties;
        }
    }
}
