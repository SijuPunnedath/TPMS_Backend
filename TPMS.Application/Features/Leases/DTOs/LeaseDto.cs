using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Deposit.DTOs;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.DTOs
{
    public class LeaseDto
    {
        
        public int LeaseID { get; set; }
        public string LeaseNumber { get; set; }
        public string LeaseName { get; set; } = string.Empty;
        public int PropertyID { get; set; }
        public string PropertyNumber { get; set; }
        public int? TenantID { get; set; }
        public int? LandlordID { get; set; }
        public int? ParentLeaseID { get; set; } // For Renewal 
        public string? TenantName { get; set; }
        public string? LandlordName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public DateTime? DateMovedIn { get; set; }
        public decimal Rent { get; set; }
        public decimal Deposit { get; set; }

        //ADded 
      //  public string Status { get; set; } 
      public LeaseStatus? Status { get; set; } = LeaseStatus.Active;
        public string PaymentFrequency { get; set; } 
 
        // new fields
        public string LeaseType { get; set; } = "Outbound";
        public string? LeaseNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal? Commission { get; set; }
        public int? PenaltyPolicyID { get; set; }

        // nested deposit (optional)
        public DepositMasterDto? DepositMaster { get; set; }
      
    }
}
