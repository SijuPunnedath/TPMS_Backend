using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RolePermissions.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

public class AssignRolePermissionsHandler
    : IRequestHandler<AssignRolePermissionsCommand, bool>
{
    private readonly TPMSDBContext _db;

    public AssignRolePermissionsHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(
        AssignRolePermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // Validate Role
        bool roleExists = await _db.Roles
            .AnyAsync(r => r.RoleID == dto.RoleID, cancellationToken);

        if (!roleExists)
            throw new KeyNotFoundException("Role not found.");

        // Load existing RolePermissions
        var existing = await _db.RolePermissions
            .Where(rp => rp.RoleID == dto.RoleID)
            .ToListAsync(cancellationToken);

        foreach (var permissionId in dto.PermissionIDs.Distinct())
        {
            var rp = existing.FirstOrDefault(x => x.PermissionID == permissionId);

            if (rp == null)
            {
                // New permission
                _db.RolePermissions.Add(new RolePermission
                {
                    RoleID = dto.RoleID,
                    PermissionID = permissionId,
                    IsAllowed = true
                });
            }
            else if (!rp.IsAllowed)
            {
                // Re-enable permission
                rp.IsAllowed = true;
                _db.RolePermissions.Update(rp);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}