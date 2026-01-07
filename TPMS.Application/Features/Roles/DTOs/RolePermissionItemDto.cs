namespace TPMS.Application.Features.Roles.DTOs;

public class RolePermissionItemDto
{
    public int PermissionID { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public bool IsAllowed { get; set; }
}