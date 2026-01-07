using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Permissions.Commands;
using TPMS.Application.Features.Permissions.DTOs;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

public class CreatePermissionHandler
    : IRequestHandler<CreatePermissionCommand, PermissionDto>
{
    private readonly TPMSDBContext _context;

    public CreatePermissionHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<PermissionDto> Handle(
        CreatePermissionCommand request,
        CancellationToken cancellationToken)
    {
        if (await _context.Permissions
                .AnyAsync(p => p.PermissionName == request.Dto.PermissionName, cancellationToken))
            throw new InvalidOperationException("Permission already exists");

        var permission = new Permission
        {
            PermissionName = request.Dto.PermissionName,
            Description = request.Dto.Description,
            Module = request.Dto.Module,
            IsSystem = true
        };

        _context.Permissions.Add(permission);
        await _context.SaveChangesAsync(cancellationToken);

        return new PermissionDto
        {
            PermissionID = permission.PermissionID,
            PermissionName = permission.PermissionName,
            Description = permission.Description,
            Module = permission.Module,
            IsSystem = permission.IsSystem
        };
    }
}