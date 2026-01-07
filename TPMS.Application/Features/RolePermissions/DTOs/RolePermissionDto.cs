using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.RolePermissions.DTOs
{
    public class RolePermissionDto
    {
        public int RolePermissionID { get; set; }
        public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public int PermissionID { get; set; }
        public string? PermissionName { get; set; } 
        public bool IsAllowed { get; set; } 
    }
}
