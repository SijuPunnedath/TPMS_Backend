using System.Collections.Generic;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Assets.DTOs;

namespace TPMS.Application.Features.Assets.Queries;

public class GetAllAssetsQuery 
    : IRequest<ApiResponse<List<AssetDto>>>
{
    
}