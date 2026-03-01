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
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Landlords.Handlers
{
    public class GetAllLandlordsHandler : IRequestHandler<GetAllLandlordsQuery, IEnumerable<LandlordDto>>
    {
        private readonly TPMSDBContext _db;

        public GetAllLandlordsHandler(TPMSDBContext db) => _db = db;

        public async Task<IEnumerable<LandlordDto>> Handle(GetAllLandlordsQuery request, CancellationToken cancellationToken)
        {
            // Lookup the OwnerTypeID for "Landlord"
            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Landlord")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            // Query landlords with their primary address
            var landlords = await _db.Landlords
                .AsNoTracking()
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

            return landlords;
        }
    }
}
