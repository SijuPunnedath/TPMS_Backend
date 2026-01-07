namespace TPMS.Application.Features.Tenants.DTOs;

public class CreateTenantDto
{
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }
    
    // Optional or mandatory nested address
    public CreateTenantAddressDto? Address { get; set; }
}