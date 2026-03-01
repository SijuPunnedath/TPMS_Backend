using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Lookups.DTOs;
using TPMS.Application.Features.Lookups.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Lookups.Handlers;

public class GetPropertyLookupHandler : IRequestHandler<GetPropertyLookupQuery, List<PropertyLookupDto>>
{
    private readonly TPMSDBContext _db;
    public GetPropertyLookupHandler(TPMSDBContext db) => _db = db;

    public async Task<List<PropertyLookupDto>> Handle(GetPropertyLookupQuery request, CancellationToken cancellationToken)
    {
        return await _db.Properties
            .OrderBy(p => p.SerialNo)
            .Select(p => new PropertyLookupDto
            {
                PropertyID = p.PropertyID,
                Label = $"{p.PropertyName}",
                LandlordID = p.LandlordID,
                LandlordName = $"{p.Landlord.Name}" 
            })
            .ToListAsync(cancellationToken);
    }  
}