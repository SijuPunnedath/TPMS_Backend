using System;

namespace TPMS.Application.Features.Reports.DTOs;

public class OverduePaymentsReportDto
{
    public int ScheduleID { get; set; }
    public int LeaseID { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int DaysLate { get; set; }
    public decimal Amount { get; set; }
    public decimal? Penalty { get; set; }
}