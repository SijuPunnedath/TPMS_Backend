using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.RolePermissions.DTOs;
using TPMS.Application.Features.RolePermissions.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RolePermissions.Handlers
{
    public class GetRolePermissionsByRoleIdHandler : IRequestHandler<GetRolePermissionsByRoleIdQuery, List<RolePermissionDto>>
    {
        private readonly TPMSDBContext _db;
        public GetRolePermissionsByRoleIdHandler(TPMSDBContext db) => _db = db;

        public async Task<List<RolePermissionDto>> Handle(GetRolePermissionsByRoleIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.RolePermissions
                .Include(r => r.Role)
                .Include(p => p.Permission)
                .Where(rp => rp.RoleID == request.RoleId)
                .Select(rp => new RolePermissionDto
                {
                    RolePermissionID = rp.RolePermissionID,
                    RoleID = rp.RoleID,
                    RoleName = rp.Role.RoleName,
                    PermissionID = rp.PermissionID,
                    PermissionName = rp.Permission.PermissionName,
                    IsAllowed = rp.IsAllowed
                })
                .ToListAsync(cancellationToken);
        }
    }
}
