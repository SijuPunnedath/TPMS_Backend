using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Domain.Entities
{
    public class RolePermission
    {
        public int RolePermissionID { get; set; }
        public int RoleID { get; set; }
        public int PermissionID { get; set; } 
        public bool IsAllowed { get; set; } = true;
        // Navigation property
        public Role? Role { get; set; } = null!; 
        public virtual Permission? Permission { get; set; } = null!;    
    }
}
