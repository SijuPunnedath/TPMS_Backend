using System;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Disputes.Commands;

public record CreateDisputeCommand(
    DisputeCategory Category,
    DisputePriority Priority,
    string Subject,
    string Description,
    DisputeReferenceType ReferenceType,
    int? ReferenceId
) : IRequest<ApiResponse<int>>;
