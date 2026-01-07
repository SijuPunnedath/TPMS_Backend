namespace TPMS.Infrastructure.Services;

public interface ICurrentUserService
{
    int UserId { get; }
    string UserName { get; }
    int? TenantId { get; }
    bool IsAuthenticated { get; }
}
