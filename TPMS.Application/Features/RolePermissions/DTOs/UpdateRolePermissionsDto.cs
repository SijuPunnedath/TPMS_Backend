using System.Collections.Generic;

namespace TPMS.Application.Features.RolePermissions.DTOs;

public class UpdateRolePermissionsDto
{
    public int RoleID { get; set; }

    // Final list of permissions that SHOULD be allowed
    public List<int> AllowedPermissionIDs { get; set; } = new();
}