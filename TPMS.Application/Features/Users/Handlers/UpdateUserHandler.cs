using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Auth.DTOs;
using TPMS.Application.Features.Users.Commands;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Users.Handlers
{
   
        public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto?>
        {
            private readonly TPMSDBContext _db;
            private readonly IPasswordHasher _passwordHasher;

            public UpdateUserHandler(TPMSDBContext db, IPasswordHasher passwordHasher)
            {
                _db = db;
                _passwordHasher = passwordHasher;
            }

            public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _db.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserID == request.UserId, cancellationToken);

                if (user == null)
                    return null;

                var dto = request.User;

                user.Username = dto.Username;
                user.Email = dto.Email ?? user.Email;
                user.RoleID = dto.RoleID;
                user.IsActive = dto.IsActive;
                user.UpdatedAt = DateTime.UtcNow;

                //-- Optional password reset
                if (!string.IsNullOrEmpty(dto.Password))
                    user.PasswordHash = _passwordHasher.Hash(dto.Password);

                await _db.SaveChangesAsync(cancellationToken);

                return new UserDto
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    Email = user.Email,
                    RoleID = user.RoleID,
                    RoleName = user.Role?.RoleName ?? "",
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                };
            }
        }
    }




