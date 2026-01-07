using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.AssetCategories.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.AssetCategories.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Infrastructure.Persistence;

public class UpdateAssetCategoryCommandHandler
    : IRequestHandler<UpdateAssetCategoryCommand, ApiResponse<bool>>
{
    private readonly TPMSDBContext _context;

    public UpdateAssetCategoryCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(
        UpdateAssetCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.AssetCategories
            .FirstOrDefaultAsync(x => x.AssetCategoryId == request.AssetCategoryId);

        if (entity == null)
            return ApiResponse<bool>.Failure("Asset category not found.");

        entity.CategoryName = request.CategoryName;
        entity.Code = request.Code;
        entity.IsDepreciable = request.IsDepreciable;
        entity.DefaultUsefulLifeMonths = request.DefaultUsefulLifeMonths;
        entity.RequiresComplianceCheck = request.RequiresComplianceCheck;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>
            .Success(true, "Asset category updated successfully.");
    }
}
