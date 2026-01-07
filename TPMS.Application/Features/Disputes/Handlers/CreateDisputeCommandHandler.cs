using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Disputes.Commands;
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Disputes.Handlers;

public class CreateDisputeCommandHandler
    : IRequestHandler<CreateDisputeCommand, ApiResponse<int>>
{
    private readonly TPMSDBContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateDisputeCommandHandler(
        TPMSDBContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<int>> Handle(
        CreateDisputeCommand request,
        CancellationToken cancellationToken)
    {
        var dispute = new Dispute
        {
           // DisputeId = Guid.NewGuid(),
            DisputeNumber = $"DSP-{DateTime.UtcNow:yyyyMMddHHmmss}",
            RaisedByUserId = _currentUser.UserId,
            RaisedBy = DisputeRaisedBy.Tenant,
            Category = request.Category,
            Priority = request.Priority,
            Subject = request.Subject,
            Description = request.Description,
            ReferenceType = request.ReferenceType,
            ReferenceId = request.ReferenceId,
            Status = DisputeStatus.Submitted,
            RaisedDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.Disputes.Add(dispute);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<int>.Success(dispute.DisputeId, "Dispute created");
    }
}
