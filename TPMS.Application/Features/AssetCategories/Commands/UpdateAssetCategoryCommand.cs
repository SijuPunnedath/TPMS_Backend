using System;
using MediatR;
using TPMS.Application.Common.Models;

namespace TPMS.Application.Features.AssetCategories.Commands;

public record UpdateAssetCategoryCommand(
    int AssetCategoryId,
    string CategoryName,
    string Code,
    bool IsDepreciable,
    int? DefaultUsefulLifeMonths,
    bool RequiresComplianceCheck,
    bool IsActive
) : IRequest<ApiResponse<bool>>;