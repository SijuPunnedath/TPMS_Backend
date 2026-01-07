using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Permissions.DTOs;
using TPMS.Application.Features.Permissions.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Permissions.Handlers;

public class GetPermissionByIdHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto?>
{
    private readonly TPMSDBContext _db;
    public GetPermissionByIdHandler(TPMSDBContext db) => _db = db;

    public async Task<PermissionDto?> Handle(
        GetPermissionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var p = await _db.Permissions
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.PermissionID == request.PermissionID,
                cancellationToken);

        return p == null ? null : new PermissionDto
        {
            PermissionID = p.PermissionID,
            PermissionName = p.PermissionName,
            Description = p.Description,
            Module = p.Module,
            IsSystem = p.IsSystem
        };
    }
}