using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Roles.DTOs;
using TPMS.Application.Features.Roles.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Roles.Handlers;

public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly TPMSDBContext _db;

    public GetRoleByIdHandler(TPMSDBContext db)
    {
        _db = db;
    }
    
    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _db.Roles.AsNoTracking()
            .FirstOrDefaultAsync(r => r.RoleID == request.RoleID, cancellationToken);

        return role == null ? null : new RoleDto
        {
            RoleID = role.RoleID,
            RoleName = role.RoleName,
            Description = role.Description,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt
        };
    }
}