using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Addresses.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Addresses.Handlers;

public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, bool>
{
    private readonly TPMSDBContext _db;
    public UpdateAddressHandler(TPMSDBContext db) => _db = db;
    
    public async Task<bool> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Address;
        var address = await _db.Addresses.FirstOrDefaultAsync(a => a.AddressID == dto.AddressID, cancellationToken);
        if (address == null) return false;

        address.AddressLine1 = dto.AddressLine1;
        address.AddressLine2 = dto.AddressLine2;
        address.City = dto.City;
        address.State = dto.State;
        address.Country = dto.Country;
        address.PostalCode = dto.PostalCode;
        address.Latitude = dto.Latitude;
        address.Longitude = dto.Longitude;
        address.Phone1 = dto.Phone1;
        address.Phone2 = dto.Phone2;
        address.Email = dto.Email;
        address.IsPrimary = dto.IsPrimary;

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

}