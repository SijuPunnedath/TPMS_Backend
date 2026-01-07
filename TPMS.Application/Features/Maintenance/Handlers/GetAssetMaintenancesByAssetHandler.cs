using System.Collections.Generic;
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

public class GetAssetMaintenancesByAssetHandler
    : IRequestHandler<GetAssetMaintenancesByAssetQuery, ApiResponse<List<AssetMaintenanceDto>>>
{
    private readonly TPMSDBContext _context;

    public GetAssetMaintenancesByAssetHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<AssetMaintenanceDto>>> Handle(
        GetAssetMaintenancesByAssetQuery request,
        CancellationToken ct)
    {
        var list = await _context.AssetMaintenances
            .Where(x => x.AssetId == request.AssetId)
            .OrderByDescending(x => x.MaintenanceDate)
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
            .ToListAsync(ct);

        return ApiResponse<List<AssetMaintenanceDto>>.Success(list, "");
    }
}
