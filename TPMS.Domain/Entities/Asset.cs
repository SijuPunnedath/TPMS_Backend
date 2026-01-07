using TPMS.Domain.Enums;

namespace TPMS.Domain.Entities;

public class Asset
{
  
   public int AssetId { get; set; }
   public int PropertyId { get; set; }

    // Future-safe (room / unit)
    public int? PropertyUnitId { get; set; }
   public int AssetCategoryId { get; set; }
    public int? AssetSubCategoryId { get; set; }

    public string AssetName { get; set; }
    public string AssetTag { get; set; }

    public AssetStatus Status { get; set; }
    public AssetCondition Condition { get; set; }

    public DateTime InstalledOn { get; set; }
    public DateTime? WarrantyExpiry { get; set; }

    public DateTime? LastServiceDate { get; set; }
    public DateTime? NextServiceDue { get; set; }

    public decimal? PurchaseValue { get; set; }

    public ICollection<AssetMaintenance> Maintenances { get; set; }
}
