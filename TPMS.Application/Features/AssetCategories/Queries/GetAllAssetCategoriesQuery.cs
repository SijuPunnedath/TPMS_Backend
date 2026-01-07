using System.Collections.Generic;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.AssetCategories.DTOs;

namespace TPMS.Application.Features.AssetCategories.Queries;

public record GetAllAssetCategoriesQuery
    : IRequest<ApiResponse<List<AssetCategoryDto>>>;