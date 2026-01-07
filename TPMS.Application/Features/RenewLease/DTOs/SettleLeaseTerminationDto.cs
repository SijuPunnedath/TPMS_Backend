namespace TPMS.Application.Features.RenewLease.DTOs;

public class SettleLeaseTerminationDto
{
    // Settled | Disputed
    public string SettlementStatus { get; set; } = string.Empty;

    public int ActionBy { get; set; }   // Logged-in UserID
    public string? Notes { get; set; }
}