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

public class GetAllAssetsQueryHandler
    : IRequestHandler<GetAllAssetsQuery, ApiResponse<List<AssetDto>>>
{
    private readonly TPMSDBContext _context;

    public GetAllAssetsQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<AssetDto>>> Handle(
        GetAllAssetsQuery request,
        CancellationToken cancellationToken)
    {
        var data = await (
                from asset in _context.Assets.AsNoTracking()
                join property in _context.Properties
                    on asset.PropertyId equals property.PropertyID
                join category in _context.AssetCategories
                    on asset.AssetCategoryId equals category.AssetCategoryId
                join subCategory in _context.AssetSubCategories
                    on asset.AssetSubCategoryId equals subCategory.AssetSubCategoryId
                    into subCategories
                from subCategory in subCategories.DefaultIfEmpty()
                select new AssetDto
                {
                    AssetId = asset.AssetId,
                    AssetName = asset.AssetName,
                    Category = category.CategoryName,
                    SubCategory = subCategory != null ? subCategory.Name : string.Empty,
                    Status = asset.Status,
                    Condition = asset.Condition,
                    InstalledOn = asset.InstalledOn,
                    NextServiceDue = asset.NextServiceDue,
                    PropertyId = asset.PropertyId,
                    PropertyName = property.PropertyName
                })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<AssetDto>>
            .Success(data, "All assets fetched successfully.");
    }
}