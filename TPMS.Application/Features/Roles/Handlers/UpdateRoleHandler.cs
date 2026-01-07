using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Roles.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Roles.Handlers;

public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, bool>
{
    private readonly TPMSDBContext _db;

    public UpdateRoleHandler(TPMSDBContext db)
    {
        _db = db;
    } 
    
    public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _db.Roles.FindAsync(new object?[] { request.RoleID }, cancellationToken);
        if (role == null)
            throw new KeyNotFoundException("Role not found.");

        if (!string.IsNullOrWhiteSpace(request.Role.RoleName))
            role.RoleName = request.Role.RoleName;

        if (request.Role.Description != null)
            role.Description = request.Role.Description;

        if (request.Role.IsActive.HasValue)
            role.IsActive = request.Role.IsActive.Value;

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}