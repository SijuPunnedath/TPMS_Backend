using System;

namespace TPMS.Application.Features.Leases.DTOs;

public class LeaseSettlementDto
{
    public int LeaseSettlementId { get; set; }
    public int LeaseId { get; set; }

    public decimal OutstandingRent { get; set; }
    public decimal DepositPaid { get; set; }
    public decimal DepositAdjusted { get; set; }
    public decimal DepositRefunded { get; set; }
    public decimal BalancePayableByTenant { get; set; }

    public string Status { get; set; }
    public DateTime SettlementDate { get; set; }
}