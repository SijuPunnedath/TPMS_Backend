using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Properties.DTOs;
using TPMS.Application.Features.Properties.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Properties.Handlers
{
    public class GetPropertyByIdHandler 
        : IRequestHandler<GetPropertyByIdQuery, PropertyDto?>
    {
        private readonly TPMSDBContext _db;

        public GetPropertyByIdHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<PropertyDto?> Handle(
            GetPropertyByIdQuery request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Resolve OwnerTypeID for Property
            var propertyOwnerTypeId = await _db.OwnerTypes
                .Where(o => o.Name == "Property")
                .Select(o => o.OwnerTypeID)
                .FirstAsync(cancellationToken);

            // 2️⃣ Projection-based query
            var property = await _db.Properties
                .AsNoTracking()
                .Where(p => p.PropertyID == request.PropertyId && !p.IsDeleted)
                .Select(p => new PropertyDto
                {
                    PropertyID = p.PropertyID,
                    PropertyNumber = p.PropertyNumber,
                    PropertyName = p.PropertyName,
                    SerialNo = p.SerialNo,
                    Type = p.Type,
                    Size = p.Size,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,

                    LandlordID = p.LandlordID,
                    LandlordName = p.Landlord != null 
                        ? p.Landlord.Name 
                        : null,

                    Address = _db.Addresses
                        .Where(a =>
                            a.OwnerTypeID == propertyOwnerTypeId &&
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
                        .FirstOrDefault() 
                        ?? new PropertyAddressDto()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return property;
        }
    }
}
