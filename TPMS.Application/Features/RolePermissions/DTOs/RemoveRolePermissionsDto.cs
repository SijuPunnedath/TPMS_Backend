using System.Collections.Generic;

namespace TPMS.Application.Features.RolePermissions.DTOs;

public class RemoveRolePermissionsDto
{
    public int RoleID { get; set; }
    public List<int> PermissionIDs { get; set; } = new();
}