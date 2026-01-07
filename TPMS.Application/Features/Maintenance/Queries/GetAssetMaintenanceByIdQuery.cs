using System;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Maintenance.DTOs;

namespace TPMS.Application.Features.Maintenance.Queries;

public record GetAssetMaintenanceByIdQuery(int AssetMaintenanceId)
    : IRequest<ApiResponse<AssetMaintenanceDto>>;
