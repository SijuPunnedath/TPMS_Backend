namespace TPMS.Application.Features.Tenants.DTOs;

public class CreateTenantAddressDto
{
    // No OwnerTypeID or OwnerID yet — these will be set after tenant creation
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string? Email { get; set; }
    public bool IsPrimary { get; set; } = true;
}