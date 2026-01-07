namespace TPMS.Application.Features.Reports.DTOs;

public class PropertyReportDto
{
    public int PropertyID { get; set; }
    public string? SerialNo { get; set; }
    public string? Type { get; set; }
    public string? Size { get; set; }
    public int LandlordID { get; set; }
    public string? LandlordName { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsOccupied { get; set; }
}