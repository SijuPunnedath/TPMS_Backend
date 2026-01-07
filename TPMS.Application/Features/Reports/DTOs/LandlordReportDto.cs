namespace TPMS.Application.Features.Reports.DTOs;

public class LandlordReportDto
{
    public int LandlordID { get; set; }
    public string? Name { get; set; }
    public string? PrimaryEmail { get; set; }
    public int PropertyCount { get; set; }
    public int ActiveLeasesCount { get; set; }
    public decimal TotalRentCollected { get; set; }
}