using System;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Assets.Commands;

public record UpdateAssetCommand(
    int AssetId,
    AssetCondition Condition,
    AssetStatus Status,
    DateTime? NextServiceDue
) : IRequest<ApiResponse<bool>>;