using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.RolePermissions.Commands;
using TPMS.Application.Features.RolePermissions.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RolePermissions.Handlers
{
    public class UpdateRolePermissionHandler : IRequestHandler<UpdateRolePermissionCommand, RolePermissionDto?>
    {
        private readonly TPMSDBContext _db;
        public UpdateRolePermissionHandler(TPMSDBContext db) => _db = db;

        public async Task<RolePermissionDto?> Handle(UpdateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var rp = await _db.RolePermissions
                .Include(r => r.Role)
                .Include(p => p.Permission)
                .FirstOrDefaultAsync(r => r.RolePermissionID == request.RolePermissionID, cancellationToken);

            if (rp == null) return null;

            rp.IsAllowed = request.IsAllowed;
            await _db.SaveChangesAsync(cancellationToken);

            return new RolePermissionDto
            {
                RolePermissionID = rp.RolePermissionID,
                RoleID = rp.RoleID,
                RoleName = rp.Role?.RoleName,
                PermissionID = rp.PermissionID,
                PermissionName = rp.Permission?.PermissionName,
                IsAllowed = rp.IsAllowed
            };
        }

    }
}
