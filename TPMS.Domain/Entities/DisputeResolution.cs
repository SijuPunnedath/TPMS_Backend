using TPMS.Domain.Enums;

namespace TPMS.Domain.Entities;

public class DisputeResolution
{
    public int DisputeResolutionId { get; set; }

    // 1 – 1 Relationship
    public int DisputeId { get; set; }
    public Dispute Dispute { get; set; } = null!;

    // Resolution Details
    public DisputeResolutionType ResolutionType { get; set; }
    public DisputeOutcome Outcome { get; set; }

    public string ResolutionSummary { get; set; } = null!;
    public string? InternalNotes { get; set; }

    // Financial Impact (Optional)
    public decimal? CompensationAmount { get; set; }
    public string? CompensationCurrency { get; set; }

    // Approval Flow
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public int? ApprovedByUserId { get; set; }
    public User? ApprovedByUser { get; set; }

    // Closure Info
    public DateTime ResolvedAt { get; set; }
    public int ResolvedByUserId { get; set; }
    public User ResolvedByUser { get; set; } = null!;

    // Audit
    public DateTime CreatedAt { get; set; }
}