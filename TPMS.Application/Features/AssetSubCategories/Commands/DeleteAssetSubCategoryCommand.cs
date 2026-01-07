using System;
using MediatR;
using TPMS.Application.Common.Models;

namespace TPMS.Application.Features.AssetSubCategories.Commands;

public record DeleteAssetSubCategoryCommand(int AssetSubCategoryId)
    : IRequest<ApiResponse<bool>>;