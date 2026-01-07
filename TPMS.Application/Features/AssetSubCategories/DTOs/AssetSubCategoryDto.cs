using System;

namespace TPMS.Application.Features.AssetSubCategories.DTOs;

public class AssetSubCategoryDto
{
    public int AssetSubCategoryId { get; set; }
    public int AssetCategoryId { get; set; }

    public string AssetCategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}