using System;
using MediatR;
using TPMS.Application.Common.Models;

namespace TPMS.Application.Features.Disputes.Commands;

public record AddDisputeCommentCommand(
    int DisputeId,
    string Comment
) : IRequest<ApiResponse<int>>;
 