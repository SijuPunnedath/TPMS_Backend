using System;
using MediatR;
using TPMS.Application.Common.Models;

namespace TPMS.Application.Features.Maintenance.Commands;

public record DeleteAssetMaintenanceCommand(int AssetMaintenanceId)
    : IRequest<ApiResponse<bool>>;