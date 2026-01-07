namespace TPMS.Domain.Entities;

public class LeaseRenewal
{
    public int LeaseRenewalID { get; set; }

    public int LeaseID { get; set; }

    public DateTime OldEndDate { get; set; }
    public DateTime NewStartDate { get; set; }
    public DateTime NewEndDate { get; set; }

    public decimal OldRent { get; set; }
    public decimal NewRent { get; set; }

    public decimal? OldDeposit { get; set; }
    public decimal? NewDeposit { get; set; }
    public decimal? AdditionalDeposit { get; set; }

    public string RenewalReason { get; set; } = string.Empty;

    public DateTime RenewedAt { get; set; } = DateTime.UtcNow;
    public int RenewedBy { get; set; }

    // Navigation
    public virtual Lease Lease { get; set; } = null!;
}
