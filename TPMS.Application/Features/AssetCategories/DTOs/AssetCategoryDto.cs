using System;

namespace TPMS.Application.Features.AssetCategories.DTOs;

public class AssetCategoryDto
{
    public int AssetCategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Code { get; set; }

    public bool IsDepreciable { get; set; }
    public int? DefaultUsefulLifeMonths { get; set; }

    public bool RequiresComplianceCheck { get; set; }
    public bool IsActive { get; set; }
}