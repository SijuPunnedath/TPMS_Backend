using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Roles.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Roles.Handlers;

public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, int>
{
    private readonly TPMSDBContext _db;

    public CreateRoleHandler(TPMSDBContext db)
    {
        _db = db;
    }
    
    public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Role;

        if (await _db.Roles.AnyAsync(r => r.RoleName == dto.RoleName, cancellationToken))
            throw new InvalidOperationException($"Role '{dto.RoleName}' already exists.");

        var role = new Role
        {
            RoleName = dto.RoleName,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _db.Roles.Add(role);
        await _db.SaveChangesAsync(cancellationToken);
        return role.RoleID;
    }
}