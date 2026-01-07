using System;

namespace TPMS.Application.Features.Reports.DTOs;

public class LeaseExpiryReportDto
{
    public int LeaseID { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string LandlordName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DaysRemaining { get; set; }
    public decimal Rent { get; set; }
}