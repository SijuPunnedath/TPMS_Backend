using System.Collections.Generic;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Assets.DTOs;

namespace TPMS.Application.Features.Assets.Queries;


public class GetAllAssetsQuery 
    : IRequest<ApiResponse<PagedResult<AssetDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
/*public class GetAllAssetsQuery 
    : IRequest<ApiResponse<List<AssetDto>>>
{
    
}*/