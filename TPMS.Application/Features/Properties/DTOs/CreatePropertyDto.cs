using System;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Properties.DTOs;

public class CreatePropertyDto
{
    public string PropertyName { get; set; } = string.Empty;
    
    public string PropertyNumber { get; set; }
    public string SerialNo { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public int LandlordID { get; set; }
    public string? LandlordName { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    
    public PropertyStatus Status { get; set; }
    public int? ActiveInboundLeaseId { get; set; }
    public int? ActiveOutboundLeaseId { get; set; }

    public PropertyAddressDto Address { get; set; } = new PropertyAddressDto();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}