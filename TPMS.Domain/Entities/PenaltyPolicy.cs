using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Domain.Entities
{
    public class PenaltyPolicy
    {
        public int PenaltyPolicyID { get; set; }
        public string Name { get; set; } = string.Empty;          // e.g. "Standard 5% Late Fee"
        public string Description { get; set; } = string.Empty;
        public decimal? FixedAmount { get; set; }                 // e.g. 500 flat
        public decimal? PercentageOfRent { get; set; }            // e.g. 5 means 5%
        public int GracePeriodDays { get; set; } = 0;             // e.g. penalty applies after 5 days
        public bool IsActive { get; set; } = true;
    }
}
