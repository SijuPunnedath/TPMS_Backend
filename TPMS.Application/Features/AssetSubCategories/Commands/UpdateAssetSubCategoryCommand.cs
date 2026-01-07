using System;
using MediatR;
using TPMS.Application.Common.Models;

namespace TPMS.Application.Features.AssetSubCategories.Commands;

public record UpdateAssetSubCategoryCommand(
    int AssetSubCategoryId,
    string Name,
    bool IsActive
) : IRequest<ApiResponse<bool>>;