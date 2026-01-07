using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Roles.DTOs;
using TPMS.Application.Features.Roles.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Roles.Handlers;

public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
{
    private readonly TPMSDBContext _db;

    public GetAllRolesHandler(TPMSDBContext db)
    {
        _db = db;
    }
    
    public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Roles.AsNoTracking();

        if (!request.IncludeInactive)
            query = query.Where(r => r.IsActive);

        var roles = await query
            .OrderBy(r => r.RoleName)
            .ToListAsync(cancellationToken);

        return roles.Select(r => new RoleDto
        {
            RoleID = r.RoleID,
            RoleName = r.RoleName,
            Description = r.Description,
            IsActive = r.IsActive,
            CreatedAt = r.CreatedAt
        }).ToList();
    }
}