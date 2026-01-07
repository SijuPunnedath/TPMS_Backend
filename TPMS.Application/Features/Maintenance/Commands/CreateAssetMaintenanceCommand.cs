using System;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Maintenance.DTOs;

namespace TPMS.Application.Features.Maintenance.Commands;

public record CreateAssetMaintenanceCommand(CreateAssetMaintenanceDto Dto)
    : IRequest<ApiResponse<int>>;
