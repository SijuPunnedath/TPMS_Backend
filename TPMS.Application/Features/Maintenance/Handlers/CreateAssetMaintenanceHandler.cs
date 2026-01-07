using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Maintenance.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Maintenance.Handlers;

public class CreateAssetMaintenanceHandler
    : IRequestHandler<CreateAssetMaintenanceCommand, ApiResponse<int>>
{
    private readonly TPMSDBContext _context;

    public CreateAssetMaintenanceHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<int>> Handle(CreateAssetMaintenanceCommand request, CancellationToken ct)
    {
        var assetExists = await _context.Assets
            .AnyAsync(x => x.AssetId == request.Dto.AssetId, ct);

        if (!assetExists)
            return ApiResponse<int>.Failure("Asset not found");

        var entity = new AssetMaintenance
        {
            //AssetMaintenanceId = Guid.NewGuid(),
            AssetId = request.Dto.AssetId,
            MaintenanceType = request.Dto.MaintenanceType,
            MaintenanceDate = request.Dto.MaintenanceDate,
            Description = request.Dto.Description,
            Cost = request.Dto.Cost,
            NextDueDate = request.Dto.NextDueDate
        };

        _context.AssetMaintenances.Add(entity);
        await _context.SaveChangesAsync(ct);

        return ApiResponse<int>.Success(entity.AssetMaintenanceId, "Maintenance record created");
    }
}
