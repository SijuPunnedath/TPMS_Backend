using System;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.AssetCategories.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.AssetCategories.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.AssetCategories.DTOs;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence;

public class CreateAssetCategoryCommandHandler
    : IRequestHandler<CreateAssetCategoryCommand, ApiResponse<AssetCategoryDto>>
{
    private readonly TPMSDBContext _context;

    public CreateAssetCategoryCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<AssetCategoryDto>> Handle(
        CreateAssetCategoryCommand request,
        CancellationToken cancellationToken)
    {
        if (await _context.AssetCategories.AnyAsync(x => x.Code == request.Code))
        {
            return ApiResponse<AssetCategoryDto>
                .Failure("Asset category code already exists.");
        }

        var entity = new AssetCategory
        {
          // AssetCategoryId = Guid.NewGuid(),
            CategoryName = request.CategoryName,
            Code = request.Code,
            IsDepreciable = request.IsDepreciable,
            DefaultUsefulLifeMonths = request.DefaultUsefulLifeMonths,
            RequiresComplianceCheck = request.RequiresComplianceCheck,
            IsActive = true
        };

        _context.AssetCategories.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<AssetCategoryDto>.Success(
            MapToDto(entity),
            "Asset category created successfully."
        );
    }

    private static AssetCategoryDto MapToDto(AssetCategory entity)
        => new()
        {
            //AssetCategoryId = entity.AssetCategoryId,
            CategoryName = entity.CategoryName,
            Code = entity.Code,
            IsDepreciable = entity.IsDepreciable,
            DefaultUsefulLifeMonths = entity.DefaultUsefulLifeMonths,
            RequiresComplianceCheck = entity.RequiresComplianceCheck,
            IsActive = entity.IsActive
        };
}
