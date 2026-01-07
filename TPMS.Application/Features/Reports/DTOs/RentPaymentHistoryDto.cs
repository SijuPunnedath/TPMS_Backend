using System;

namespace TPMS.Application.Features.Reports.DTOs;

public class RentPaymentHistoryDto
{
    public int ScheduleID { get; set; }
    public int LeaseID { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public DateTime PaidDate { get; set; }
    public decimal Amount { get; set; }
    public decimal? Penalty { get; set; }
}