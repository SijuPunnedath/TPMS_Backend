using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Maintenance.DTOs;
using TPMS.Application.Features.Maintenance.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Maintenance.Handlers;

public class GetAssetMaintenanceByIdHandler
    : IRequestHandler<GetAssetMaintenanceByIdQuery, ApiResponse<AssetMaintenanceDto>>
{
    private readonly TPMSDBContext _context;

    public GetAssetMaintenanceByIdHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<AssetMaintenanceDto>> Handle(
        GetAssetMaintenanceByIdQuery request,
        CancellationToken ct)
    {
        var result = await _context.AssetMaintenances
            .Where(x => x.AssetMaintenanceId == request.AssetMaintenanceId)
            .Select(x => new AssetMaintenanceDto
            {
                AssetMaintenanceId = x.AssetMaintenanceId,
                AssetId = x.AssetId,
                MaintenanceType = x.MaintenanceType,
                MaintenanceDate = x.MaintenanceDate,
                Description = x.Description,
                Cost = x.Cost,
                NextDueDate = x.NextDueDate
            })
            .FirstOrDefaultAsync(ct);

        if (result == null)
            return ApiResponse<AssetMaintenanceDto>.Failure("Maintenance record not found");

        return ApiResponse<AssetMaintenanceDto>.Success(result, "Asset Maintenance record successfully");
    }
}
