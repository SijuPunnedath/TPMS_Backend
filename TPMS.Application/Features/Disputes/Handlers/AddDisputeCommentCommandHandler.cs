using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Disputes.Commands;
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Disputes.Handlers;

public class AddDisputeCommentCommandHandler
    : IRequestHandler<AddDisputeCommentCommand, ApiResponse<int>>
{
    private readonly TPMSDBContext _context;
    private readonly ICurrentUserService _currentUser;

    public AddDisputeCommentCommandHandler(
        TPMSDBContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<int>> Handle(
        AddDisputeCommentCommand request,
        CancellationToken cancellationToken)
    {
        var dispute = await _context.Disputes
            .FirstOrDefaultAsync(x => x.DisputeId == request.DisputeId, cancellationToken);

        if (dispute == null)
            return ApiResponse<int>.Failure("Dispute not found");

        if (dispute.Status == DisputeStatus.Closed)
            return ApiResponse<int>.Failure("Cannot comment on closed dispute");

        var comment = new DisputeComment
        {
            //DisputeCommentId = Guid.NewGuid(),
            DisputeId = request.DisputeId,
            CommentedByUserId = _currentUser.UserId,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.DisputeComments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<int>.Success(comment.DisputeCommentId, "Comment added");
    }
}
