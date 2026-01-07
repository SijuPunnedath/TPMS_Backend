using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Domain.Entities
{
    public class User
         {
             public int UserID { get; set; }
             public string Username { get; set; } = string.Empty;
             public string Email { get; set; } = string.Empty;
             public string PasswordHash { get; set; } = string.Empty;
             public int RoleID { get; set; }
             public Role? Role { get; set; }
             public bool IsActive { get; set; } = true;
             public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
             public DateTime? UpdatedAt { get; set; }
             public DateTime? LastLoginAt { get; set; }
     
             public ICollection<RefreshToken>? RefreshTokens { get; set; }
         }
}
