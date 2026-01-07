namespace TPMS.Domain.Entities;

public class DepositTransaction
{
    public int DepositTransactionID { get; set; }

    public int DepositMasterID { get; set; }
    public virtual DepositMaster DepositMaster { get; set; } = null!;

    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }

    public string Type { get; set; } = "Payment"; 
    // Payment, Refund, Adjustment
    public string? Notes { get; set; }
}