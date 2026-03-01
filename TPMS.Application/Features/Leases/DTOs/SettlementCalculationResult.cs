namespace TPMS.Application.Features.Leases.DTOs;

public class SettlementCalculationResult
{
    public decimal OutstandingRent { get; set; }
    public decimal DepositPaid { get; set; }
    public decimal DepositAdjusted { get; set; }
    public decimal DepositRefunded { get; set; }
    public decimal BalancePayableByTenant { get; set; }
}