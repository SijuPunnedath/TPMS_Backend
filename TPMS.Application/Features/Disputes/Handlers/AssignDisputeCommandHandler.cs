using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Disputes.Commands;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Disputes.Handlers;

public class AssignDisputeCommandHandler 
    : IRequestHandler<AssignDisputeCommand>
{
    private readonly TPMSDBContext _context;

    public AssignDisputeCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task Handle(
        AssignDisputeCommand request, 
        CancellationToken cancellationToken)
    {
        var dispute = await _context.Disputes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (dispute == null)
            throw new Exception("Dispute not found");

        dispute.AssignedToUserId = request.AssignedToUserId;
        dispute.Status = DisputeStatus.UnderReview;

        await _context.SaveChangesAsync(cancellationToken);
    }
}