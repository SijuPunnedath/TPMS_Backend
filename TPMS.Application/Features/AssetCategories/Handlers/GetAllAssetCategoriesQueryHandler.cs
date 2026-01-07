using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.AssetCategories.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.AssetCategories.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.AssetCategories.DTOs;
using TPMS.Infrastructure.Persistence;

public class GetAllAssetCategoriesQueryHandler
    : IRequestHandler<GetAllAssetCategoriesQuery, ApiResponse<List<AssetCategoryDto>>>
{
    private readonly TPMSDBContext _context;

    public GetAllAssetCategoriesQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<AssetCategoryDto>>> Handle(
        GetAllAssetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _context.AssetCategories
            .AsNoTracking()
            .Select(x => new AssetCategoryDto
            {
                AssetCategoryId = x.AssetCategoryId,
                CategoryName = x.CategoryName,
                Code = x.Code,
                IsDepreciable = x.IsDepreciable,
                DefaultUsefulLifeMonths = x.DefaultUsefulLifeMonths,
                RequiresComplianceCheck = x.RequiresComplianceCheck,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<AssetCategoryDto>>
            .Success(data, "Asset categories fetched successfully.");
    }
}
