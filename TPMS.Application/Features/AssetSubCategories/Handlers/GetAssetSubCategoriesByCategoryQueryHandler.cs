using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.AssetSubCategories.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.AssetSubCategories.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.AssetSubCategories.DTOs;
using TPMS.Infrastructure.Persistence;

public class GetAssetSubCategoriesByCategoryQueryHandler
    : IRequestHandler<GetAssetSubCategoriesByCategoryQuery, ApiResponse<List<AssetSubCategoryDto>>>
{
    private readonly TPMSDBContext _context;

    public GetAssetSubCategoriesByCategoryQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<AssetSubCategoryDto>>> Handle(
        GetAssetSubCategoriesByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _context.AssetSubCategories
            .Where(x => x.AssetCategoryId == request.AssetCategoryId)
            .AsNoTracking()
            .Select(x => new AssetSubCategoryDto
            {
                AssetSubCategoryId = x.AssetSubCategoryId,
                AssetCategoryId = x.AssetCategoryId,
                AssetCategoryName = x.AssetCategory.CategoryName,
                Name = x.Name,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<AssetSubCategoryDto>>
            .Success(data, "Asset subcategories fetched successfully.");
    }
}
