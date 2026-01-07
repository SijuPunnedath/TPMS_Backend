using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Domain.Entities
{
    public class UserRole
    {
        public int UserRoleID { get; set; }     // PK
        public int UserID { get; set; }         // FK to Users
        public int RoleID { get; set; }         // FK to Roles
        public DateTime AssignedAt { get; set; } 
        public bool IsActive { get; set; } = true;

        public virtual User? User { get; set; }
        public virtual Role? Role { get; set; }
    }
}
