using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.UserRoles.DTOs
{
    public class UserRoleDto
    {
        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public string? Username { get; set; }
        public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public DateTime AssignedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
