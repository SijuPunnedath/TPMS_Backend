namespace TPMS.Application.Features.Permissions.DTOs;

public class CreatePermissionDto
{
    public string PermissionName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
}