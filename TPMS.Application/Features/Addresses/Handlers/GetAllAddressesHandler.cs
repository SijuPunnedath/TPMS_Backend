using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Addresses.DTOs;
using TPMS.Application.Features.Addresses.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Addresses.Handlers;

public class GetAllAddressesHandler : IRequestHandler<GetAllAddressesQuery, List<AddressDto>>
{
    private readonly TPMSDBContext _db;
    public GetAllAddressesHandler(TPMSDBContext db) => _db = db;
    
    public async Task<List<AddressDto>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
    {
        return await _db.Addresses
            .Select(a => new AddressDto
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
            })
            .OrderBy(a => a.OwnerTypeID)
            .ToListAsync(cancellationToken);
    }
}