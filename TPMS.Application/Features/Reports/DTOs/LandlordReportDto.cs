namespace TPMS.Application.Features.Reports.DTOs;

/*
public class LandlordReportDto
{
    public int LandlordID { get; set; }
    public string? Name { get; set; }
    public string? PrimaryEmail { get; set; }
    public int PropertyCount { get; set; }
    public int ActiveLeasesCount { get; set; }
    public decimal TotalRentCollected { get; set; }
}
*/


public class LandlordReportDto
{
    public int LandlordID { get; set; }
    public string Name { get; set; }

    public string PrimaryEmail { get; set; }

    // Address (Primary)
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }

    public int PropertyCount { get; set; }
    public int ActiveLeasesCount { get; set; }
    public decimal TotalRentCollected { get; set; }
}

