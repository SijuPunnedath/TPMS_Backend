using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Roles.DTOs;
using TPMS.Application.Features.Roles.Queries;

namespace TPMS.Application.Features.Roles.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Infrastructure.Persistence.Configurations;

public class GetAllRolesWithPermissionsHandler
    : IRequestHandler<GetAllRolesWithPermissionsQuery, List<RoleWithPermissionsDto>>
{
    private readonly TPMSDBContext _db;

    public GetAllRolesWithPermissionsHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<List<RoleWithPermissionsDto>> Handle(
        GetAllRolesWithPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var rolePermissions = await _db.RolePermissions
            .AsNoTracking()
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .ToListAsync(cancellationToken);

        return rolePermissions
            .GroupBy(rp => new { rp.RoleID, rp.Role!.RoleName })
            .Select(g => new RoleWithPermissionsDto
            {
                RoleID = g.Key.RoleID,
                RoleName = g.Key.RoleName,
                Permissions = g
                    .Select(rp => new RolePermissionItemDto
                    {
                        PermissionID = rp.PermissionID,
                        PermissionName = rp.Permission!.PermissionName,
                        IsAllowed = rp.IsAllowed
                    })
                    .OrderBy(p => p.PermissionName)
                    .ToList()
            })
            .OrderBy(r => r.RoleName)
            .ToList();
    }

}
