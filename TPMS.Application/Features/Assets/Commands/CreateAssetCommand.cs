using System;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Assets.DTOs;

namespace TPMS.Application.Features.Assets.Commands;

public record CreateAssetCommand(CreateAssetDto Dto)
    : IRequest<ApiResponse<int>>;