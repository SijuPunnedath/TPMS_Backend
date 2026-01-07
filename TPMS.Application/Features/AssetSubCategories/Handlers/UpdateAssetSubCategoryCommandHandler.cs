using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.AssetSubCategories.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.AssetSubCategories.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Infrastructure.Persistence;

public class UpdateAssetSubCategoryCommandHandler
    : IRequestHandler<UpdateAssetSubCategoryCommand, ApiResponse<bool>>
{
    private readonly TPMSDBContext _context;

    public UpdateAssetSubCategoryCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(
        UpdateAssetSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.AssetSubCategories
            .FirstOrDefaultAsync(x => x.AssetSubCategoryId == request.AssetSubCategoryId, cancellationToken);

        if (entity == null)
            return ApiResponse<bool>.Failure("Asset subcategory not found.");

        entity.Name = request.Name;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>
            .Success(true, "Asset subcategory updated successfully.");
    }
}
