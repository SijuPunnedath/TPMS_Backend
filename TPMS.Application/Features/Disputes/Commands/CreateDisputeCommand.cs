using System;
using MediatR;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Disputes.Commands;

using TPMS.Application.Common.Models;

public record CreateDisputeCommand(
    string DisputeNumber,
    int TenantId,
    int RaisedByUserId,
    DisputeRaisedBy RaisedBy,
    DisputeCategory Category,
    DisputePriority Priority,
    string Subject,
    string Description,
    DisputeReferenceType ReferenceType,
    int? ReferenceId,
    DateTime? DueDate
) : IRequest<ApiResponse<int>>;