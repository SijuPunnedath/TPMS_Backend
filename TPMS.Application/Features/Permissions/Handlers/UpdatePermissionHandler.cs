using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Permissions.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

public class UpdatePermissionHandler
    : IRequestHandler<UpdatePermissionCommand, bool>
{
    private readonly TPMSDBContext _context;

    public UpdatePermissionHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        UpdatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        var permission = await _context.Permissions
            .FindAsync(new object[] { request.PermissionId }, cancellationToken);

        if (permission == null)
            return false;

        if (permission.IsSystem)
            throw new InvalidOperationException("System permission cannot be modified");

        permission.PermissionName = request.Dto.PermissionName;
        permission.Description = request.Dto.Description;
        permission.Module = request.Dto.Module;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}