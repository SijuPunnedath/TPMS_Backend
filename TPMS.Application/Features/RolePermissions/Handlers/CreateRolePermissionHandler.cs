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
    public class CreateRolePermissionHandler : IRequestHandler<CreateRolePermissionCommand, RolePermissionDto>
    {
        private readonly TPMSDBContext _db;
        public CreateRolePermissionHandler(TPMSDBContext db) => _db = db;

        public async Task<RolePermissionDto> Handle(CreateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            var exists = await _db.RolePermissions
                .AnyAsync(rp => rp.RoleID == request.RoleID && rp.PermissionID == request.PermissionID, cancellationToken);
            if (exists)
                throw new InvalidOperationException("Permission already assigned to this role.");

            var entity = new Domain.Entities.RolePermission
            {
                RoleID = request.RoleID,
                PermissionID = request.PermissionID,
                IsAllowed = request.isAllowed
            };

            _db.RolePermissions.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            var role = await _db.Roles.FindAsync(entity.RoleID);
            var permission = await _db.Permissions.FindAsync(entity.PermissionID);

            return new RolePermissionDto
            {
                RolePermissionID = entity.RolePermissionID,
                RoleID = entity.RoleID,
                RoleName = role?.RoleName,
                PermissionID = entity.PermissionID,
                PermissionName = permission?.PermissionName,
                IsAllowed = entity.IsAllowed
            };
        }

    }
}

//public int RolePermissionID { get; set; }
//public int RoleID { get; set; }
//public string? RoleName { get; set; }
//public int PermissionID { get; set; }
//public string? PermissionName { get; set; }
//public bool IsAllowed { get; set; }