using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.OwnerTypes.DTOs;
using TPMS.Application.Features.OwnerTypes.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.OwnerTypes.Handlers;

public class GetAllOwnerTypesHandler : IRequestHandler<GetAllOwnerTypesQuery, List<OwnerTypeDto>>
{
    private readonly TPMSDBContext _db;
    public GetAllOwnerTypesHandler(TPMSDBContext db) => _db = db;

    public async Task<List<OwnerTypeDto>> Handle(GetAllOwnerTypesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.OwnerTypes.AsQueryable();
        if (!request.IncludeInactive)
            query = query.Where(o => o.IsActive);

        return await query
            .Select(o => new OwnerTypeDto
            {
                OwnerTypeID = o.OwnerTypeID,
                Name = o.Name,
                Description = o.Description,
                IsActive = o.IsActive
            })
            .OrderBy(o => o.Name)
            .ToListAsync(cancellationToken);
    } 
}