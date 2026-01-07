using System;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Assets.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Assets.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence;

public class CreateAssetCommandHandler
    : IRequestHandler<CreateAssetCommand, ApiResponse<int>>
{
    private readonly TPMSDBContext _context;

    public CreateAssetCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<int>> Handle(
        CreateAssetCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (await _context.Assets.AnyAsync(x => x.AssetTag == dto.AssetTag))
            return ApiResponse<int>.Failure("Asset tag already exists.");

        var asset = new Asset
        {
           // AssetId = Guid.NewGuid(),
            PropertyId = dto.PropertyId,
            AssetCategoryId = dto.AssetCategoryId,
            AssetSubCategoryId = dto.AssetSubCategoryId,
            AssetName = dto.AssetName,
            AssetTag = dto.AssetTag,
            InstalledOn = dto.InstalledOn,
            WarrantyExpiry = dto.WarrantyExpiry,
            PurchaseValue = dto.PurchaseValue,

            Status = AssetStatus.Installed,
            Condition = AssetCondition.Good
        };

        _context.Assets.Add(asset);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<int>
            .Success(asset.AssetId, "Asset created successfully.");
    }
}
