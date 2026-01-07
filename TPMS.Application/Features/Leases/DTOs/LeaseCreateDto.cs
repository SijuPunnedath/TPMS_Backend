using System;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.DTOs
{
    public class LeaseCreateDto
    {
        public string LeaseName { get; set; } = string.Empty;

        public int PropertyID { get; set; }

        // Only one is required depending on LeaseType
        public int? TenantID { get; set; }     // OUTBOUND
        public int? LandlordID { get; set; }   // INBOUND

        public LeaseType LeaseType { get; set; }   // 🔒 Enum

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public DateTime? DateMovedIn { get; set; }

        public decimal Rent { get; set; }
        public decimal Deposit { get; set; }

        public string Status { get; set; } = "Active";
        public string PaymentFrequency { get; set; } = "Monthly";

        public decimal? Commission { get; set; }
        public decimal? GuaranteedRent { get; set; }

        public int? PenaltyPolicyID { get; set; }

        public string? DisputeNotes { get; set; }
        public string? LeaseNotes { get; set; }
    }
}