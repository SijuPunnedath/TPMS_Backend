using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Domain.Entities
{
    public class RefreshToken
    {
        public int TokenID { get; set; }
        public string Token { get; set; } = string.Empty;
        public int UserID { get; set; }
        public User? User { get; set; }

        public DateTime Expires { get; set; }
        public bool Revoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }

        public DateTime CreatedAt { get; set; } 
        public string? CreatedByIp { get; set; }

        public bool IsActive => !Revoked && DateTime.UtcNow < Expires;
    }
}
