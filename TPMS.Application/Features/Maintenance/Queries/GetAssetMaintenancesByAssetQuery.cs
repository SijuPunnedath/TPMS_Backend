using System;
using System.Collections.Generic;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Maintenance.DTOs;

namespace TPMS.Application.Features.Maintenance.Queries;

public record GetAssetMaintenancesByAssetQuery(int AssetId)
    : IRequest<ApiResponse<List<AssetMaintenanceDto>>>;
