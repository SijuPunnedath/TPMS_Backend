using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Permissions.DTOs;
using TPMS.Application.Features.Permissions.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Permissions.Handlers;

public class GetAllPermissionsHandler : IRequestHandler<GetAllPermissionsQuery, List<PermissionDto>>
{

    private readonly TPMSDBContext _db;
    public GetAllPermissionsHandler(TPMSDBContext db) => _db = db;

    public async Task<List<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _db.Permissions
            .AsNoTracking()
            .OrderBy(p => p.Module)
            .ThenBy(p => p.PermissionName)
            .Select(p => new PermissionDto
            {
                PermissionID = p.PermissionID,
                PermissionName = p.PermissionName,
                Description = p.Description,
                Module = p.Module,
                IsSystem = p.IsSystem
            })
            .ToListAsync(cancellationToken);
    }
}