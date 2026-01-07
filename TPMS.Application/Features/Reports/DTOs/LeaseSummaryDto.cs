using System;

namespace TPMS.Application.Features.Reports.DTOs;

public class LeaseSummaryDto
{
    public int LeaseID { get; set; }
    public int PropertyID { get; set; }
    public string? PropertySerialNo { get; set; }
    public int TenantID { get; set; }
    public string? TenantName { get; set; }
    public int LandlordID { get; set; }
    public string? LandlordName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Rent { get; set; }
    public string? Status { get; set; }
}