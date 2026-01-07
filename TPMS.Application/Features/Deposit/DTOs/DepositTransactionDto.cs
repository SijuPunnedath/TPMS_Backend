using System;

namespace TPMS.Application.Features.Deposit.DTOs;

public class DepositTransactionDto
{
    public int DepositTransactionID { get; set; }
    public int DepositMasterID { get; set; }

    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Type { get; set; } = "Payment";
    public string? Notes { get; set; }  
}