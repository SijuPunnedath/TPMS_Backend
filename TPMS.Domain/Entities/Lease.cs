using System;
using System.Collections.Generic;
using TPMS.Domain.Enums;

namespace TPMS.Domain.Entities
{
    public class Lease
    {
        public int LeaseID { get; set; }

        public string LeaseName { get; set; } = string.Empty;
        public int PropertyID { get; set; }

        // Nullable because they depend on LeaseType
        public int? TenantID { get; set; }       // OUTBOUND only
        public int? LandlordID { get; set; }     // INBOUND only

        public LeaseType LeaseType { get; set; } // 🔒 ENUM (CORE FIX)

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public DateTime? DateMovedIn { get; set; }

        // Rent meaning depends on LeaseType
        public decimal Rent { get; set; }
        public decimal Deposit { get; set; }

        public decimal? Commission { get; set; } = 0;
        public decimal? GuaranteedRent { get; set; } = 0;

        public string Status { get; set; } = "Active"; // Active | Renewed | Terminated | Expired
        public string PaymentFrequency { get; set; } = "Monthly";

        public int? PenaltyPolicyID { get; set; }
        public virtual PenaltyPolicy? PenaltyPolicy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual Property Property { get; set; } = null!;
        public virtual Tenant? Tenant { get; set; }
        public virtual Landlord? Landlord { get; set; }

        public virtual ICollection<RentSchedule> RentSchedules { get; set; } = new List<RentSchedule>();
        public virtual ICollection<LeaseAlert> LeaseAlerts { get; set; } = new List<LeaseAlert>();
        public virtual DepositMaster? DepositMaster { get; set; }

        
        // ==============================
        // Lifecycle & Dispute Info
        // ==============================

        public string? EndReason { get; set; }

        public decimal? Deductions { get; set; }
        public string? DeductionReason { get; set; }

        public string? DisputeNotes { get; set; }

        public decimal? OverdueAmount { get; set; }
        public string? OverdueReason { get; set; }

        public string? LeaseNotes { get; set; }
        
        // Newly added for termination and renewal
        public bool IsTerminated { get; set; } = false;
        public DateTime? TerminatedAt { get; set; }
        
        public virtual ICollection<LeaseRenewal> Renewals { get; set; }
            = new List<LeaseRenewal>();

        public virtual LeaseTermination? Termination { get; set; }

    }
}
