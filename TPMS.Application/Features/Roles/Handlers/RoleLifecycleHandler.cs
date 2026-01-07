using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Roles.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Roles.Handlers;

public class RoleLifecycleHandler :
                    IRequestHandler<SoftDeleteRoleCommand, bool>,
                    IRequestHandler<RestoreRoleCommand, bool>,
                    IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly TPMSDBContext _db;

    public RoleLifecycleHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(SoftDeleteRoleCommand request, CancellationToken ct)
    {
        var role = await _db.Roles.FindAsync(new object?[] { request.RoleID }, ct);
        if (role == null) throw new KeyNotFoundException("Role not found.");

        role.IsActive = false;
        await _db.SaveChangesAsync(ct);
        return true;
    } 
    
    public async Task<bool> Handle(RestoreRoleCommand request, CancellationToken ct)
    {
        var role = await _db.Roles.FindAsync(new object?[] { request.RoleID }, ct);
        if (role == null) throw new KeyNotFoundException("Role not found.");

        role.IsActive = true;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken ct)
    {
        var role = await _db.Roles.FindAsync(new object?[] { request.RoleID }, ct);
        if (role == null) throw new KeyNotFoundException("Role not found.");

        _db.Roles.Remove(role);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}