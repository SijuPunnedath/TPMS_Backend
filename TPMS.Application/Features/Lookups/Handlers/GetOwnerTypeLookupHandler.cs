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

public class GetOwnerTypeLookupHandler : IRequestHandler<GetOwnerTypeLookupQuery, List<OwnerTypeLookupDto>>
{
    private readonly TPMSDBContext _db;
    public GetOwnerTypeLookupHandler(TPMSDBContext db) => _db = db;

    public async Task<List<OwnerTypeLookupDto>> Handle(GetOwnerTypeLookupQuery request, CancellationToken cancellationToken)
    {
        return await _db.OwnerTypes
            .Where(o => o.IsActive)
            .OrderBy(o => o.Name)
            .Select(o => new OwnerTypeLookupDto
            {
                OwnerTypeID = o.OwnerTypeID,
                Name = o.Name!
            })
            .ToListAsync(cancellationToken);
    }
}