using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Assets.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Assets.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence;

public class DeleteAssetCommandHandler
    : IRequestHandler<DeleteAssetCommand, ApiResponse<bool>>
{
    private readonly TPMSDBContext _context;

    public DeleteAssetCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(
        DeleteAssetCommand request,
        CancellationToken cancellationToken)
    {
        var asset = await _context.Assets
            .FirstOrDefaultAsync(x => x.AssetId == request.AssetId, cancellationToken);

        if (asset == null)
            return ApiResponse<bool>.Failure("Asset not found.");

        asset.Status = AssetStatus.Disposed;

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>
            .Success(true, "Asset retired successfully.");
    }
}
