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
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly TPMSDBContext _db;
        public GetAllUsersHandler(TPMSDBContext db) { _db = db; }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users.Include(u => u.Role)
                .Select(u => new UserDto
                {
                    UserID = u.UserID,
                    Username = u.Username,
                    Email = u.Email,
                    RoleID = u.RoleID,
                    RoleName = u.Role != null ? u.Role.RoleName : "",
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                }).ToListAsync(cancellationToken);
        }       
    }
}
