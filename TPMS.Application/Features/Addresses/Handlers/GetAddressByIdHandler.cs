using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Addresses.DTOs;
using TPMS.Application.Features.Addresses.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Addresses.Handlers;

public class GetAddressByIdHandler : IRequestHandler<GetAddressByIdQuery, AddressDto?>
{
    private readonly TPMSDBContext _db;
    public GetAddressByIdHandler(TPMSDBContext db) => _db = db;
    public async Task<AddressDto?> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var a = await _db.Addresses.FirstOrDefaultAsync(x => x.AddressID == request.AddressID, cancellationToken);
        if (a == null) return null;

        return new AddressDto
        {
            AddressID = a.AddressID,
            OwnerTypeID = a.OwnerTypeID,
            OwnerID = a.OwnerID,
            AddressLine1 = a.AddressLine1,
            AddressLine2 = a.AddressLine2,
            City = a.City,
            State = a.State,
            Country = a.Country,
            PostalCode = a.PostalCode,
            Latitude = a.Latitude,
            Longitude = a.Longitude,
            Phone1 = a.Phone1,
            Phone2 = a.Phone2,
            Email = a.Email,
            IsPrimary = a.IsPrimary
        };
    }
}
