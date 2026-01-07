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

public class GetRoleLookupHandler : IRequestHandler<GetRoleLookupQuery, List<RoleLookupDto>>
{
    private readonly TPMSDBContext _db;
    public GetRoleLookupHandler(TPMSDBContext db) => _db = db;

    public async Task<List<RoleLookupDto>> Handle(GetRoleLookupQuery request, CancellationToken cancellationToken)
    {
        return await _db.Roles
            .Where(r => r.IsActive)
            .OrderBy(r => r.RoleName)
            .Select(r => new RoleLookupDto
            {
                RoleID = r.RoleID,
                RoleName = r.RoleName
            })
            .ToListAsync(cancellationToken);
    }
}