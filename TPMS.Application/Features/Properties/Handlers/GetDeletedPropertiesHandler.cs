using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Application.Features.Properties.DTOs;
using TPMS.Application.Features.Properties.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Properties.Handlers
{
    public class GetDeletedPropertiesHandler : IRequestHandler<GetDeletedPropertiesQuery, IEnumerable<PropertyDto>>
    {
        private readonly TPMSDBContext _db;
        public GetDeletedPropertiesHandler(TPMSDBContext db) => _db = db;

        public async Task<IEnumerable<PropertyDto>> Handle(GetDeletedPropertiesQuery request, CancellationToken cancellationToken)
        {
            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Property")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            return await _db.Properties
                .AsNoTracking()
                .Where(p => p.IsDeleted)
                .Select(p => new PropertyDto
                {
                    PropertyID = p.PropertyID,
                    SerialNo = p.SerialNo,
                    Type = p.Type,
                    Size = p.Size,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Address = _db.Addresses
                        .Where(a => a.OwnerTypeID == ownerTypeId && a.OwnerID == p.PropertyID && a.IsPrimary)
                        .Select(a => new PropertyAddressDto()
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
                        .FirstOrDefault() ?? new PropertyAddressDto()
                })
                .ToListAsync(cancellationToken);
        }
    }
}
