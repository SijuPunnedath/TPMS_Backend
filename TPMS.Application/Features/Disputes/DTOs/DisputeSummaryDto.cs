namespace TPMS.Application.Features.Disputes.DTOs;

public class DisputeSummaryDto
{
    public int Total { get; set; }
    public int Open { get; set; }
    public int InProgress { get; set; }
    public int Closed { get; set; }
    public int Escalated { get; set; }
}