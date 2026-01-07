using System.Collections.Generic;

namespace TPMS.Application.Features.Roles.DTOs;

public class RoleWithPermissionsDto
{
    public int RoleID { get; set; }
    public string RoleName { get; set; } = string.Empty;

    public List<RolePermissionItemDto> Permissions { get; set; } = new();
}