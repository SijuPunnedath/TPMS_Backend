using System;
using System.Collections.Generic;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.AssetSubCategories.DTOs;

namespace TPMS.Application.Features.AssetSubCategories.Queries;

public record GetAssetSubCategoriesByCategoryQuery(int AssetCategoryId)
    : IRequest<ApiResponse<List<AssetSubCategoryDto>>>;