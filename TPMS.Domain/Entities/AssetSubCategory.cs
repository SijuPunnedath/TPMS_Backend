namespace TPMS.Domain.Entities;

public class AssetSubCategory
{
    public int AssetSubCategoryId { get; set; }

    public int AssetCategoryId { get; set; }
    public AssetCategory AssetCategory { get; set; }

    public string Name { get; set; }
    public bool IsActive { get; set; } = true;
}