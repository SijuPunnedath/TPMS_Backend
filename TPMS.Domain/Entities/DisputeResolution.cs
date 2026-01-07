using TPMS.Domain.Enums;

namespace TPMS.Domain.Entities;

public class DisputeResolution
{
    public int DisputeResolutionId { get; set; }

    public int DisputeId { get; set; }
    public Dispute Dispute { get; set; }

    public DisputeResolutionType ResolutionType { get; set; }
    public string ResolutionSummary { get; set; }

    public decimal? AdjustedAmount { get; set; }
    public int? AdjustedInvoiceId { get; set; }

    public int ResolvedByUserId { get; set; }
    public DateTime ResolvedAt { get; set; }
}
