namespace TPMS.Application.Features.Properties.DTOs;

public class PropertyAddressDto
{
    public int? AddressID { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string? Email { get; set; }
    public bool IsPrimary { get; set; }
}