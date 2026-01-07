using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.UserRoles.DTOs;
using TPMS.Application.Features.UserRoles.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.UserRoles.Handlers
{
    public class GetAllUserRolesHandler : IRequestHandler<GetAllUserRolesQuery, List<UserRoleDto>>
    {
        private readonly TPMSDBContext _db;

        public GetAllUserRolesHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<List<UserRoleDto>> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
        {
            var data = await _db.UserRoles
                .Include(u => u.User)
                .Include(r => r.Role)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return data.Select(ur => new UserRoleDto
            {
                UserRoleID = ur.UserRoleID,
                UserID = ur.UserID,
                Username = ur.User?.Username,
                RoleID = ur.RoleID,
                RoleName = ur.Role?.RoleName,
                AssignedAt = ur.AssignedAt,
                IsActive = ur.IsActive
            }).ToList();
        }
    }
}
