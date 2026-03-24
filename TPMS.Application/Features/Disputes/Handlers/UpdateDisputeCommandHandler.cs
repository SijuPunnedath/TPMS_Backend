using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Disputes.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Disputes.Handlers;

public class UpdateDisputeCommandHandler 
    : IRequestHandler<UpdateDisputeCommand>
{
    private readonly TPMSDBContext _context;

    public UpdateDisputeCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateDisputeCommand request, 
        CancellationToken cancellationToken)
    {
        var dispute = await _context.Disputes
            .FirstOrDefaultAsync(
                x => x.DisputeId == request.Id && x.DeletedAt == null,
                cancellationToken);

        if (dispute == null)
            throw new Exception("Dispute not found");

        dispute.Category = request.Category;
        dispute.Priority = request.Priority;
        dispute.Subject = request.Subject;
        dispute.Description = request.Description;
        dispute.DueDate = request.DueDate;
        dispute.UpdatedAt = DateTime.UtcNow;
        dispute.UpdatedByUserId = request.UpdatedByUserId;

        await _context.SaveChangesAsync(cancellationToken);
    }
}