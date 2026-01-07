using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user, IList<Claim>? additionalClaims = null);
        RefreshToken GenerateRefreshToken(string ipAddress, TimeSpan refreshTtl);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
