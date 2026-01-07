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
    public class CreateUserRoleHandler : IRequestHandler<CreateUserRoleCommand, UserRoleDto>
    {
        private readonly TPMSDBContext _db;

        public CreateUserRoleHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<UserRoleDto> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var exists = await _db.UserRoles
                .AnyAsync(ur => ur.UserID == request.UserID && ur.RoleID == request.RoleID && ur.IsActive, cancellationToken);

            if (exists)
                throw new InvalidOperationException("This role is already assigned to the user.");

            var userRole = new Domain.Entities.UserRole
            {
                UserID = request.UserID,
                RoleID = request.RoleID,
                AssignedAt = DateTime.UtcNow,
                IsActive = true
            };

            _db.UserRoles.Add(userRole);
            await _db.SaveChangesAsync(cancellationToken);

            var role = await _db.Roles.FindAsync(userRole.RoleID);
            var user = await _db.Users.FindAsync(userRole.UserID);

            return new UserRoleDto
            {
                UserRoleID = userRole.UserRoleID,
                UserID = userRole.UserID,
                Username = user?.Username,
                RoleID = userRole.RoleID,
                RoleName = role?.RoleName,
                AssignedAt = userRole.AssignedAt,
                IsActive = true
            };
        }
    }
}
