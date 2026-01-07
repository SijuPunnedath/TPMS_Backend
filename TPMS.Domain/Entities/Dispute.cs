using TPMS.Domain.Enums;

namespace TPMS.Domain.Entities;

public class Dispute
{
    public int DisputeId { get; set; }

    public string DisputeNumber { get; set; } // DSP-2026-0001

    public int RaisedByUserId { get; set; }
    public User RaisedByUser { get; set; }

    public DisputeRaisedBy RaisedBy { get; set; } // Tenant, Owner, Admin

    public DisputeCategory Category { get; set; }
    public DisputeStatus Status { get; set; }
    public DisputePriority Priority { get; set; }

    public string Subject { get; set; }
    public string Description { get; set; }

    // Reference
    public DisputeReferenceType ReferenceType { get; set; }
    public int? ReferenceId { get; set; } // InvoiceId, MaintenanceId, LeaseId

    public DateTime RaisedDate { get; set; }
    public DateTime? DueDate { get; set; }

    public int? AssignedToUserId { get; set; }
    public User AssignedToUser { get; set; }

    public bool IsEscalated { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    
    public ICollection<DisputeComment> Comments { get; set; } = new List<DisputeComment>();
    public ICollection<DisputeAttachment> Attachments { get; set; } = new List<DisputeAttachment>();

    public DisputeResolution Resolution { get; set; } //  1–1 relationship
}
