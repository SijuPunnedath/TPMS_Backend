using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Auth.Commands;
using TPMS.Application.Features.Auth.DTOs;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Auth.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResponseDto>
    {
        private readonly TPMSDBContext _db;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenService _jwt;
        private readonly IConfiguration _config;

        public LoginUserHandler(TPMSDBContext db, IPasswordHasher hasher, IJwtTokenService jwt, IConfiguration config)
        {
            _db = db;
            _hasher = hasher;
            _jwt = jwt;
            _config = config;
        }

        public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == dto.Username, cancellationToken);
            if (user == null || !_hasher.Verify(dto.Password, user.PasswordHash) || !user.IsActive)
                throw new UnauthorizedAccessException("Invalid credentials.");

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);

            // Generate tokens
            var access = _jwt.GenerateAccessToken(user);
            var refreshTtl = TimeSpan.FromDays(int.Parse(_config["Jwt:RefreshTokenDays"] ?? "7"));
            var refresh = _jwt.GenerateRefreshToken(request.IpAddress, refreshTtl);

            // persist refresh
            refresh.UserID = user.UserID;
            _db.RefreshTokens.Add(refresh);
            await _db.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto
            {
                AccessToken = access,
                RefreshToken = refresh.Token,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:AccessTokenMinutes"] ?? "15")),
                UserID = user.UserID,
                Username = user.Username,
                Role = user.Role?.RoleName ?? ""
            };
        }


    }
}
