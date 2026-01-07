using System;

namespace TPMS.Application.Features.Reports.DTOs;

public class RentDueReportDto
{
    public int ScheduleID { get; set; }
    public int LeaseID { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
}