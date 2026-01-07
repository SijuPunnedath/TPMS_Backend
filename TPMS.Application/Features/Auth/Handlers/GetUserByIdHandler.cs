using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Auth.DTOs;
using TPMS.Application.Features.Auth.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Auth.Handlers
{
    public  class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly TPMSDBContext _db;

        public GetUserByIdHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserID == request.UserId, cancellationToken);

            if (user == null)
                return null;

            return new UserDto
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                RoleID = user.RoleID,
                RoleName = user.Role?.RoleName ?? string.Empty,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

    }
}
