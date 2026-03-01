using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Lookups.DTOs;
using TPMS.Application.Features.Lookups.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Lookups.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Domain.Enums;

public class GetAvailableInboundPropertiesHandler 
    : IRequestHandler<GetAvailableInboundPropertiesQuery, List<PropertyLookupDto>>
{
    private readonly TPMSDBContext _db;

    public GetAvailableInboundPropertiesHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<List<PropertyLookupDto>> Handle(
        GetAvailableInboundPropertiesQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.Properties
            .AsNoTracking()
            .Where(p =>
                !p.IsDeleted &&
                p.ActiveInboundLeaseId == null)
            .Select(p => new PropertyLookupDto
            {
                PropertyID = p.PropertyID,
                Label = p.PropertyName,
                LandlordID = p.LandlordID,
                LandlordName = p.Landlord != null 
                    ? p.Landlord.Name 
                    : null
            })
            .ToListAsync(cancellationToken);
    }
}