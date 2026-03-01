using System;
using TPMS.Domain.Enums;

namespace TPMS.Domain.Entities
{
    public class LeaseSettlement
    {
        public int LeaseSettlementId { get; set; }

        // -----------------------------
        // Relationship
        // -----------------------------
        public int LeaseId { get; set; }
        public Lease Lease { get; set; } = null!;

        // -----------------------------
        // Settlement Metadata
        // -----------------------------
        public DateTime SettlementDate { get; set; }
        public SettlementStatus Status { get; set; }

        // -----------------------------
        // Financial Breakdown
        // -----------------------------
        public decimal OutstandingRent { get; set; }
        public decimal PenaltyAmount { get; set; }
        public decimal DamageCharges { get; set; }

        public decimal DepositPaid { get; set; }
        public decimal DepositAdjusted { get; set; }
        public decimal DepositRefunded { get; set; }
        public decimal BalancePayableByTenant { get; set; }

        // -----------------------------
        // Optional Notes / Audit
        // -----------------------------
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? SettledAt { get; set; }
        public string? SettledBy { get; set; }
    }
}