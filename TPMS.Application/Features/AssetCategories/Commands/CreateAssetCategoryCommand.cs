using System;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.AssetCategories.DTOs;

namespace TPMS.Application.Features.AssetCategories.Commands;

public record CreateAssetCategoryCommand(
    string CategoryName,
    string Code,
    bool IsDepreciable,
    int? DefaultUsefulLifeMonths,
    bool RequiresComplianceCheck
) : IRequest<ApiResponse<AssetCategoryDto>>;