namespace TPMS.Application.Features.Settings.DTOs;

public class CompanySettingsDto
{
    public int CompanyID { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? RegistrationNumber { get; set; }
    public string? TaxID { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Email { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string? Website { get; set; }
    public string? LogoUrl { get; set; }
    public string? Currency { get; set; }
    public string? TimeZone { get; set; }   
}