namespace TPMS.Domain.Entities;

public class AssetCategory
{
    public int AssetCategoryId { get; set; }

    public string CategoryName { get; set; }
    public string Code { get; set; }

    public bool IsDepreciable { get; set; }
    public int? DefaultUsefulLifeMonths { get; set; }

    public bool RequiresComplianceCheck { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<AssetSubCategory> SubCategories { get; set; }
}