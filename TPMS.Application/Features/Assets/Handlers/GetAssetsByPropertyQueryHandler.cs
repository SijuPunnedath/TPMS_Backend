using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Assets.DTOs;
using TPMS.Application.Features.Assets.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Assets.Handlers;

public class GetAssetsByPropertyQueryHandler
    : IRequestHandler<GetAssetsByPropertyQuery, ApiResponse<List<AssetDto>>>
{
    private readonly TPMSDBContext _context;

    public GetAssetsByPropertyQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<AssetDto>>> Handle(
        GetAssetsByPropertyQuery request,
        CancellationToken cancellationToken)
    {
        var data = await (
                from asset in _context.Assets.AsNoTracking()
                join category in _context.AssetCategories
                    on asset.AssetCategoryId equals category.AssetCategoryId
                join subCategory in _context.AssetSubCategories
                    on asset.AssetSubCategoryId equals subCategory.AssetSubCategoryId
                    into subCategories
                from subCategory in subCategories.DefaultIfEmpty()
                where asset.PropertyId == request.PropertyId
                select new AssetDto
                {
                    AssetId = asset.AssetId,
                    AssetName = asset.AssetName,
                    Category = category.CategoryName,
                    SubCategory = subCategory != null ? subCategory.Name : string.Empty,
                    Status = asset.Status,
                    Condition = asset.Condition,
                    InstalledOn = asset.InstalledOn,
                    NextServiceDue = asset.NextServiceDue
                })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<AssetDto>>
            .Success(data, "Assets fetched successfully.");
    }
}
