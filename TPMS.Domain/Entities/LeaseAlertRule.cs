using TPMS.Domain.Enums;

namespace TPMS.Domain.Entities;

public class LeaseAlertRule
{
    public int RuleID { get; set; }

    public string RuleCode { get; set; } = string.Empty;
    // LEASE_EXPIRY, RENT_DUE, RENT_OVERDUE, DEPOSIT_REFUND

    public string AlertType { get; set; } = string.Empty;
    // LeaseExpiry, RentDue, RentOverdue

    public int TriggerDays { get; set; }
    // Example: 30, 15, 7, 0, -5 (after due date)

    public LeaseType? LeaseType { get; set; }
    // INBOUND / OUTBOUND / NULL = ALL

    public string PaymentFrequency { get; set; } = "Monthly";

    public bool IsActive { get; set; } = true;

    public string DeliveryMethod { get; set; } = "Dashboard";
    // Email, SMS, Dashboard (CSV or JSON also ok)

    public string MessageTemplate { get; set; } = string.Empty;

    public TimeSpan RunTime { get; set; }
    // 02:00 AM execution preference

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
