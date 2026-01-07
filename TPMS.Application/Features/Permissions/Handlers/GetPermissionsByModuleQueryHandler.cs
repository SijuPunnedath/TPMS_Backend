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

public class GetPermissionsByModuleQueryHandler : IRequestHandler<GetPermissionsByModuleQuery, List<PermissionDto>>
{
    private readonly TPMSDBContext _context;

    public GetPermissionsByModuleQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<PermissionDto>> Handle(
        GetPermissionsByModuleQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Permissions
            .AsNoTracking()
            .Where(p => p.Module == request.Module)
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