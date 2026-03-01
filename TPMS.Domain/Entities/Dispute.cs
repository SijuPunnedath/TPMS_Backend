
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;

public class Dispute
{
    public int DisputeId { get; set; }
    public string DisputeNumber { get; set; }

    // SaaS Isolation
    public int TenantId { get; set; }

    // Who Raised
    public int RaisedByUserId { get; set; }
    public DisputeRaisedBy RaisedBy { get; set; }
    public User RaisedByUser { get; set; } = null!;

    // Classification
    public DisputeCategory Category { get; set; }
    public DisputeStatus Status { get; set; }
    public DisputePriority Priority { get; set; }

    // Content
    public string Subject { get; set; }
    public string Description { get; set; }

    // Reference
    public DisputeReferenceType ReferenceType { get; set; }
    public int? ReferenceId { get; set; }

    // Assignment
    public int? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }
    public int EscalationLevel { get; set; }

    // Dates
    public DateTime RaisedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ClosedAt { get; set; }

    // Audit
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedByUserId { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public bool RequiresFollowUp { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public bool IsEscalated { get; set; }

    // Navigation
    public ICollection<DisputeComment> Comments { get; set; }
    public ICollection<DisputeAttachment> Attachments { get; set; }
    public DisputeResolution Resolution { get; set; }
}
