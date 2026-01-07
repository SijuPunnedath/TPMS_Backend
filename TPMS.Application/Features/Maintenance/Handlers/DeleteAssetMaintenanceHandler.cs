using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Maintenance.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Maintenance.Handlers;

public class DeleteAssetMaintenanceHandler
    : IRequestHandler<DeleteAssetMaintenanceCommand, ApiResponse<bool>>
{
    private readonly TPMSDBContext _context;

    public DeleteAssetMaintenanceHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteAssetMaintenanceCommand request, CancellationToken ct)
    {
        var entity = await _context.AssetMaintenances
            .FirstOrDefaultAsync(x => x.AssetMaintenanceId == request.AssetMaintenanceId, ct);

        if (entity == null)
            return ApiResponse<bool>.Failure("Maintenance record not found");

        _context.AssetMaintenances.Remove(entity);
        await _context.SaveChangesAsync(ct);

        return ApiResponse<bool>.Success(true, "Maintenance record deleted");
    }
}
