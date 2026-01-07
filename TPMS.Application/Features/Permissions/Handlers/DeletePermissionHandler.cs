using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Permissions.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

public class DeletePermissionHandler
    : IRequestHandler<DeletePermissionCommand, bool>
{
    private readonly TPMSDBContext _context;

    public DeletePermissionHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        DeletePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var permission = await _context.Permissions
            .Include(p => p.RolePermissions)
            .FirstOrDefaultAsync(
                p => p.PermissionID == request.PermissionID,
                cancellationToken);

        if (permission == null)
            return false;

        if (permission.IsSystem)
            throw new InvalidOperationException("System permission cannot be deleted");

        if (permission.RolePermissions.Any())
            throw new InvalidOperationException("Permission assigned to roles");

        _context.Permissions.Remove(permission);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}