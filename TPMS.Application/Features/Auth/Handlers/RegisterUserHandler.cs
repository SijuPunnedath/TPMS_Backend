using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Auth.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Auth.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly TPMSDBContext _db;
        private readonly IPasswordHasher _hasher;

        public RegisterUserHandler(TPMSDBContext db, IPasswordHasher hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            if (await _db.Users.AnyAsync(u => u.Username == dto.Username, cancellationToken))
                throw new InvalidOperationException("Username already exists.");

            var roleId = dto.RoleID ?? (await _db.Roles.Where(r => r.RoleName == "Tenant").Select(r => r.RoleID).FirstOrDefaultAsync(cancellationToken));

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = _hasher.Hash(dto.Password),
                RoleID = roleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);
            return user.UserID;
        }
    }
}
