using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Application.Features.Landlords.Queries;
using TPMS.Infrastructure.Persistence;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Landlords.Handlers
{
    public class GetLandlordByIdHandler : IRequestHandler<GetLandlordByIdQuery, LandlordDto?>
    {
        private readonly TPMSDBContext _db;
        public GetLandlordByIdHandler(TPMSDBContext db) => _db = db;

        public async Task<LandlordDto?> Handle(GetLandlordByIdQuery request, CancellationToken cancellationToken)
        {
         
            var landlord = await _db.Landlords
           .AsNoTracking()
           .FirstOrDefaultAsync(l => l.LandlordID == request.LandlordId, cancellationToken);

            if (landlord == null) return null;

            //-- Find OwnerTypeID for "Landlord"
            var ownerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Landlord")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            //-- Load the primary address for this landlord
            var address = await _db.Addresses
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.OwnerTypeID == ownerTypeId
                                       && a.OwnerID == landlord.LandlordID
                                       && a.IsPrimary, cancellationToken);

            return new LandlordDto
            {
                LandlordID = landlord.LandlordID,
                LandlordNumber = landlord.LandlordNumber,
                Name = landlord.Name ?? string.Empty,
                Notes = landlord.Notes,
                CreatedAt = landlord.CreatedAt,
                UpdatedAt = landlord.UpdatedAt,
                LandlordAddress = address == null
                ? new LandlordAddressDto() // empty if not found
                : new LandlordAddressDto
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    Country = address.Country,
                    PostalCode = address.PostalCode,
                    Phone1 = address.Phone1,
                    Phone2 = address.Phone2,
                    Email = address.Email,
                    IsPrimary = address.IsPrimary
                }
        };

        }
    }
}
