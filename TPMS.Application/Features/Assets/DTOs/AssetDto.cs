using System;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Assets.DTOs;

public class AssetDto
{
    public int AssetId { get; set; }
    public string AssetName { get; set; }
    
    public int PropertyId { get; set; }
    public string PropertyName { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }

    public AssetStatus Status { get; set; }
    public AssetCondition Condition { get; set; }

    public DateTime InstalledOn { get; set; }
    public DateTime? NextServiceDue { get; set; }
}