using System.Collections.Generic;

namespace TPMS.Application.Features.RolePermissions.DTOs;

public class AssignRolePermissionsDto
{
    public int RoleID { get; set; }
    public List<int> PermissionIDs { get; set; } = new();
}