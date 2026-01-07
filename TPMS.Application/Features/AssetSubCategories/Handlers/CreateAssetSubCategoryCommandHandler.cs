using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.AssetSubCategories.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.AssetSubCategories.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.AssetSubCategories.DTOs;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence;

public class CreateAssetSubCategoryCommandHandler
    : IRequestHandler<CreateAssetSubCategoryCommand, ApiResponse<AssetSubCategoryDto>>
{
    private readonly TPMSDBContext _context;

    public CreateAssetSubCategoryCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<AssetSubCategoryDto>> Handle(
        CreateAssetSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryExists = await _context.AssetCategories
            .AnyAsync(x => x.AssetCategoryId == request.AssetCategoryId, cancellationToken);

        if (!categoryExists)
            return ApiResponse<AssetSubCategoryDto>
                .Failure("Invalid asset category.");

        var duplicate = await _context.AssetSubCategories.AnyAsync(
            x => x.AssetCategoryId == request.AssetCategoryId &&
                 x.Name == request.Name,
            cancellationToken);

        if (duplicate)
            return ApiResponse<AssetSubCategoryDto>
                .Failure("Asset subcategory already exists under this category.");

        var entity = new AssetSubCategory
        {
           // AssetSubCategoryId = Guid.NewGuid(),
            AssetCategoryId = request.AssetCategoryId,
            Name = request.Name,
            IsActive = true
        };

        _context.AssetSubCategories.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = await MapToDto(entity.AssetSubCategoryId, cancellationToken);

        return ApiResponse<AssetSubCategoryDto>
            .Success(dto, "Asset subcategory created successfully.");
    }

    private async Task<AssetSubCategoryDto> MapToDto(int id, CancellationToken ct)
    {
        return await _context.AssetSubCategories
            .Where(x => x.AssetSubCategoryId == id)
            .Select(x => new AssetSubCategoryDto
            {
                AssetSubCategoryId = x.AssetSubCategoryId,
                AssetCategoryId = x.AssetCategoryId,
                AssetCategoryName = x.AssetCategory.CategoryName,
                Name = x.Name,
                IsActive = x.IsActive
            })
            .FirstAsync(ct);
    }
}
