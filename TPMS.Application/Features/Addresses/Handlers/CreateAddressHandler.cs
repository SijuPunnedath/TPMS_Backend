using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Addresses.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Addresses.Handlers;

public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, int>
{
    private readonly TPMSDBContext _db;
    public CreateAddressHandler(TPMSDBContext db) => _db = db; 
    
    public async Task<int> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Address;
        var address = new Address
        {
            OwnerTypeID = dto.OwnerTypeID,
            OwnerID = dto.OwnerID,
            AddressLine1 = dto.AddressLine1,
            AddressLine2 = dto.AddressLine2,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            PostalCode = dto.PostalCode,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Phone1 = dto.Phone1,
            Phone2 = dto.Phone2,
            Email = dto.Email,
            IsPrimary = dto.IsPrimary
        };

        _db.Addresses.Add(address);
        await _db.SaveChangesAsync(cancellationToken);
        return address.AddressID;
    }
}