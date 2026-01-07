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
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly TPMSDBContext _db;
        private readonly IJwtTokenService _jwt;
        private readonly IConfiguration _config;

        public RefreshTokenHandler(TPMSDBContext db, IJwtTokenService jwt, IConfiguration config)
        {
            _db = db;
            _jwt = jwt;
            _config = config;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var existing = await _db.RefreshTokens.Include(t => t.User).ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(t => t.Token == request.Token, cancellationToken);

            if (existing == null || !existing.IsActive)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            // rotate tokens: revoke existing and create new
            existing.Revoked = true;
            existing.RevokedAt = DateTime.UtcNow;
            var refreshTtl = TimeSpan.FromDays(int.Parse(_config["Jwt:RefreshTokenDays"] ?? "7"));
            var newRefresh = _jwt.GenerateRefreshToken(request.IpAddress, refreshTtl);
            newRefresh.UserID = existing.UserID;
            existing.ReplacedByToken = newRefresh.Token;

            _db.RefreshTokens.Add(newRefresh);
            await _db.SaveChangesAsync(cancellationToken);

            var access = _jwt.GenerateAccessToken(existing.User!);

            return new AuthResponseDto
            {
                AccessToken = access,
                RefreshToken = newRefresh.Token,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:AccessTokenMinutes"] ?? "15")),
                UserID = existing.User!.UserID,
                Username = existing.User.Username,
                Role = existing.User.Role?.RoleName ?? ""
            };
        }
    }

}

