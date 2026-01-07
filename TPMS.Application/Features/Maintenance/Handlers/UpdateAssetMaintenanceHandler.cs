using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Maintenance.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Maintenance.Handlers;

public class UpdateAssetMaintenanceHandler
    : IRequestHandler<UpdateAssetMaintenanceCommand, ApiResponse<bool>>
{
    private readonly TPMSDBContext _context;

    public UpdateAssetMaintenanceHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateAssetMaintenanceCommand request, CancellationToken ct)
    {
        var entity = await _context.AssetMaintenances
            .FirstOrDefaultAsync(x => x.AssetMaintenanceId == request.Dto.AssetMaintenanceId, ct);

        if (entity == null)
            return ApiResponse<bool>.Failure("Maintenance record not found");

        entity.MaintenanceType = request.Dto.MaintenanceType;
        entity.MaintenanceDate = request.Dto.MaintenanceDate;
        entity.Description = request.Dto.Description;
        entity.Cost = request.Dto.Cost;
        entity.NextDueDate = request.Dto.NextDueDate;

        await _context.SaveChangesAsync(ct);

        return ApiResponse<bool>.Success(true, "Maintenance record updated");
    }
}
