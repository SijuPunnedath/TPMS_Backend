using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.UserRoles.Commands;
using TPMS.Application.Features.UserRoles.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.UserRoles.Handlers
{
    public class UpdateUserRoleHandler : IRequestHandler<UpdateUserRoleCommand, UserRoleDto?>
    {
        private readonly TPMSDBContext _db;

        public UpdateUserRoleHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<UserRoleDto?> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var userRole = await _db.UserRoles
                .Include(ur => ur.Role)
                .Include(ur => ur.User)
                .FirstOrDefaultAsync(ur => ur.UserRoleID == request.UserRoleID, cancellationToken);

            if (userRole == null)
                return null;

            userRole.RoleID = request.RoleID;
            userRole.IsActive = request.IsActive;

            await _db.SaveChangesAsync(cancellationToken);

            return new UserRoleDto
            {
                UserRoleID = userRole.UserRoleID,
                UserID = userRole.UserID,
                Username = userRole.User?.Username,
                RoleID = userRole.RoleID,
                RoleName = userRole.Role?.RoleName,
                IsActive = userRole.IsActive,
                AssignedAt = userRole.AssignedAt
            };
        }

    }
}
