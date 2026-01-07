using System;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.AssetSubCategories.DTOs;

namespace TPMS.Application.Features.AssetSubCategories.Commands;

public record CreateAssetSubCategoryCommand(
    int AssetCategoryId,
    string Name
) : IRequest<ApiResponse<AssetSubCategoryDto>>;