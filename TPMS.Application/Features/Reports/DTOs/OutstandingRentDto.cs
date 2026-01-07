using System;

namespace TPMS.Application.Features.Reports.DTOs;

public class OutstandingRentDto
{
    public int TenantID { get; set; }
    public string? TenantName { get; set; }
    public int LeaseID { get; set; }
    public int PropertyID { get; set; }
    public string? PropertySerialNo { get; set; }
    public DateTime DueDate { get; set; }
    public int DaysOverdue { get; set; }
    public decimal Amount { get; set; }
    public decimal? Penalty { get; set; }
}