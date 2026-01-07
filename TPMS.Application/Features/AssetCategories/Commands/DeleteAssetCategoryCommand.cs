using System;
using MediatR;

namespace TPMS.Application.Features.AssetCategories.Commands;

public class DeleteAssetCategoryCommand : IRequest
{
    public Guid AssetCategoryId { get; set; }
}