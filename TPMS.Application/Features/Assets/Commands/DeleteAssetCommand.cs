using System;
using MediatR;
using TPMS.Application.Common.Models;

namespace TPMS.Application.Features.Assets.Commands;

public record DeleteAssetCommand(int AssetId)
    : IRequest<ApiResponse<bool>>;