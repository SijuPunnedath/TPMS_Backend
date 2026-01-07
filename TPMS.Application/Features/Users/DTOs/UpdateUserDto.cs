using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Users.DTOs
{
    public class UpdateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int RoleID { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Password { get; set; }  // optional, if password needs reset
    }
}
