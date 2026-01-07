namespace TPMS.Domain.Entities;

public class DepositMaster 
{
    public int DepositMasterID { get; set; }

    public int LeaseID { get; set; }
    public virtual Lease Lease { get; set; } = null!;

    public decimal ExpectedAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceAmount { get; set; }

    public string Status { get; set; } = "Pending"; 
    public string? Notes { get; set; }

    public virtual ICollection<DepositTransaction> Transactions { get; set; }
        = new List<DepositTransaction>();
}