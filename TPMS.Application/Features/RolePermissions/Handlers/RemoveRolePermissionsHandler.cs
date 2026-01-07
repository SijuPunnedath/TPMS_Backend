using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RolePermissions.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

public class RemoveRolePermissionsHandler
    : IRequestHandler<RemoveRolePermissionsCommand, bool>
{
    private readonly TPMSDBContext _db;

    public RemoveRolePermissionsHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(
        RemoveRolePermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var rolePermissions = await _db.RolePermissions
            .Where(rp =>
                rp.RoleID == dto.RoleID &&
                dto.PermissionIDs.Contains(rp.PermissionID) &&
                rp.IsAllowed)
            .ToListAsync(cancellationToken);

        foreach (var rp in rolePermissions)
        {
            rp.IsAllowed = false;
        }

        if (rolePermissions.Any())
        {
            _db.RolePermissions.UpdateRange(rolePermissions);
            await _db.SaveChangesAsync(cancellationToken);
        }

        return true;
    }
}