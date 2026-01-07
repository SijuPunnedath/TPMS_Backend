using System.Collections.Generic;

namespace TPMS.Application.Features.Deposit.DTOs;

public class DepositMasterDto
{
    public int DepositMasterID { get; set; }
    public int LeaseID { get; set; }

    public decimal ExpectedAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public string? Notes { get; set; }

    public List<DepositTransactionDto> Transactions { get; set; } = new();
}