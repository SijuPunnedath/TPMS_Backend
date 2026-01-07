using System;
using MediatR;
using TPMS.Application.Common.Models;

namespace TPMS.Application.Features.Disputes.Commands;

public record AddDisputeAttachmentCommand(
    int DisputeId,
    int DocumentId,
    string FileName
) : IRequest<ApiResponse<int>>;
