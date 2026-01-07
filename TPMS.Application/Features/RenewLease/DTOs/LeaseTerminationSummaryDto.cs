using System;

namespace TPMS.Application.Features.RenewLease.DTOs;

public class LeaseTerminationSummaryDto
{
    public int LeaseTerminationID { get; set; }
    public int LeaseID { get; set; }

    public DateTime TerminationDate { get; set; }
    public DateTime EffectiveEndDate { get; set; }

    public string TerminationType { get; set; } = string.Empty;
    public string TerminationReason { get; set; } = string.Empty;

    public decimal OutstandingRent { get; set; }
    public decimal PenaltyAmount { get; set; }
    public decimal DamageCharges { get; set; }

    public decimal DepositAdjusted { get; set; }
    public decimal DepositRefunded { get; set; }

    public string SettlementStatus { get; set; } = string.Empty;

    // Lease context
    public string LeaseName { get; set; } = string.Empty;
    public decimal Rent { get; set; }
    public decimal Deposit { get; set; }

    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
}