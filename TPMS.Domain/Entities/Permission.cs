using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Domain.Entities
{
    public class Permission
    {
        public int PermissionID { get; set; }

        public string PermissionName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Module { get; set; } = string.Empty;
        public bool IsSystem { get; set; } = true;

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
            = new List<RolePermission>();
    }
}
