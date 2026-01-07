using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TPMS.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public int UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not authenticated");

            return Convert.ToInt32(userId);
        }
    }
    
    public string UserName =>
        _httpContextAccessor.HttpContext?
            .User?
            .Identity?
            .Name ?? string.Empty;

    
    public int? TenantId
    {
        get
        {
            var tenantId = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst("TenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantId))
                return null;

            return int.TryParse(tenantId, out var id) ? id : null;
        }
    }
    
    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?
            .User?
            .Identity?
            .IsAuthenticated ?? false;

}