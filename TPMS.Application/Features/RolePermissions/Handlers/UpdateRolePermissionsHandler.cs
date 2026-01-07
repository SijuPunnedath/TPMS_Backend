using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.RolePermissions.Commands;
using TPMS.Domain.Entities;

namespace TPMS.Application.Features.RolePermissions.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Infrastructure.Persistence.Configurations;

public class UpdateRolePermissionsHandler
    : IRequestHandler<UpdateRolePermissionsCommand, bool>
{
    private readonly TPMSDBContext _db;

    public UpdateRolePermissionsHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(
        UpdateRolePermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // Load all permissions for role
        var existingPermissions = await _db.RolePermissions
            .Where(rp => rp.RoleID == dto.RoleID)
            .ToListAsync(cancellationToken);

        var allowedSet = dto.AllowedPermissionIDs.Distinct().ToHashSet();

        // 1️⃣ Enable or create selected permissions
        foreach (var permissionId in allowedSet)
        {
            var rp = existingPermissions
                .FirstOrDefault(x => x.PermissionID == permissionId);

            if (rp == null)
            {
                _db.RolePermissions.Add(new RolePermission
                {
                    RoleID = dto.RoleID,
                    PermissionID = permissionId,
                    IsAllowed = true
                });
            }
            else if (!rp.IsAllowed)
            {
                rp.IsAllowed = true;
            }
        }

        // 2️⃣ Disable unselected permissions
        foreach (var rp in existingPermissions)
        {
            if (!allowedSet.Contains(rp.PermissionID) && rp.IsAllowed)
            {
                rp.IsAllowed = false;
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
