using System;
using MediatR;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Disputes.Commands;

public record UpdateDisputeCommand(
    int Id,
    DisputeCategory Category,
    DisputePriority Priority,
    string Subject,
    string Description,
    DateTime? DueDate,
    int UpdatedByUserId
) : IRequest;