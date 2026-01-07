using System;

namespace TPMS.Application.Features.Assets.DTOs;

public class CreateAssetDto
{
    public int PropertyId { get; set; }
    public int AssetCategoryId { get; set; }
    public int? AssetSubCategoryId { get; set; }

    public string AssetName { get; set; }
    public string AssetTag { get; set; }

    public DateTime InstalledOn { get; set; }
    public DateTime? WarrantyExpiry { get; set; }
    public decimal? PurchaseValue { get; set; }
}