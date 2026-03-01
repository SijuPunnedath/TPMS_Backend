using System;

namespace TPMS.Application.Features.Landlords.DTOs;

public class CreateLandlordDto
{
   
    public string Name { get; set; } = string.Empty;
    public string LandlordNumber { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public LandlordAddressDto LandlordAddress { get; set; } = new LandlordAddressDto();
}