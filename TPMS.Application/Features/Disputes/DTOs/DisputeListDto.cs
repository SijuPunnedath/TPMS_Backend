using System;

namespace TPMS.Application.Features.Disputes.DTOs;

public class DisputeListDto
{
    public int DisputeId { get; set; }
    public string DisputeNumber { get; set; }
    public string Subject { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public string Category { get; set; }
    public DateTime RaisedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsEscalated { get; set; }
    public string RaisedByUser { get; set; }
    public string? AssignedToUser { get; set; }
}