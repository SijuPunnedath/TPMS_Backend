using System;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Disputes.DTOs;

public class DisputeDto
{
    public int DisputeId { get; set; }
    public string DisputeNumber { get; set; }

    public int TenantId { get; set; }

    public DisputeCategory Category { get; set; }
    public DisputeStatus Status { get; set; }
    public DisputePriority Priority { get; set; }

    public string Subject { get; set; }
    public string Description { get; set; }

    public DateTime RaisedDate { get; set; }
    public DateTime? DueDate { get; set; }

    public int? AssignedToUserId { get; set; }
    public bool IsEscalated { get; set; }

    public DateTime? ClosedAt { get; set; }
}