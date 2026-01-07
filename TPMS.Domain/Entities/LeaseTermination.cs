namespace TPMS.Domain.Entities;

public class LeaseTermination
{
    public int LeaseTerminationID { get; set; }

    public int LeaseID { get; set; }

    public DateTime TerminationDate { get; set; }
    public DateTime EffectiveEndDate { get; set; }

    // Mutual | Tenant | Landlord | System
    public string TerminationType { get; set; } = string.Empty;

    public string TerminationReason { get; set; } = string.Empty;

    public decimal OutstandingRent { get; set; }
    public decimal PenaltyAmount { get; set; }
    public decimal DamageCharges { get; set; }

    public decimal DepositAdjusted { get; set; }
    public decimal DepositRefunded { get; set; }

    // Pending | Settled | Disputed
    public string SettlementStatus { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int CreatedBy { get; set; }
    
    //Settlement fields
    public DateTime? SettledAt { get; set; }
    public int? SettledBy { get; set; }
    
    public string? Notes { get; set; }

    // Navigation
    public virtual Lease Lease { get; set; } = null!;
}