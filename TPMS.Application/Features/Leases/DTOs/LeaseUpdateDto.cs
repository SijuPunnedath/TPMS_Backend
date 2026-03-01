using System;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.DTOs
{
    public class LeaseUpdateDto
    {
        public int LeaseID { get; set; }

        public string LeaseName { get; set; } = string.Empty;
        public int PropertyID { get; set; }

        // Can change only if business allows
        public int? TenantID { get; set; }
        public int? LandlordID { get; set; }

        public LeaseType LeaseType { get; set; }   // 🔒 Explicit change
        public int? ParentLeaseID { get; set; } // For Renewal 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public DateTime? DateMovedIn { get; set; }

        public decimal Rent { get; set; }
        public decimal Deposit { get; set; }

       // public string Status { get; set; }
       public LeaseStatus? Status { get; set; } = LeaseStatus.Active;
        public string PaymentFrequency { get; set; }

        public decimal? Commission { get; set; }
        public decimal? GuaranteedRent { get; set; }

        public int? PenaltyPolicyID { get; set; }

        public string? EndReason { get; set; }
        public string? DisputeNotes { get; set; }
        public string? LeaseNotes { get; set; }
    }
}