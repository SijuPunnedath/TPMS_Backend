using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.AssetSubCategories.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.AssetSubCategories.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Infrastructure.Persistence;

public class DeleteAssetSubCategoryCommandHandler
    : IRequestHandler<DeleteAssetSubCategoryCommand, ApiResponse<bool>>
{
    private readonly TPMSDBContext _context;

    public DeleteAssetSubCategoryCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(
        DeleteAssetSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.AssetSubCategories
            .FirstOrDefaultAsync(
                x => x.AssetSubCategoryId == request.AssetSubCategoryId,
                cancellationToken);

        if (entity == null)
            return ApiResponse<bool>
                .Failure("Asset subcategory not found.");

        if (!entity.IsActive)
            return ApiResponse<bool>
                .Failure("Asset subcategory is already inactive.");
        var isUsed = await _context.Assets
            .AnyAsync(x => x.AssetSubCategoryId == request.AssetSubCategoryId);

        if (isUsed)
            return ApiResponse<bool>
                .Failure("Cannot delete subcategory because assets are mapped to it.");

        entity.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>
            .Success(true, "Asset subcategory deleted successfully.");
    }
}
