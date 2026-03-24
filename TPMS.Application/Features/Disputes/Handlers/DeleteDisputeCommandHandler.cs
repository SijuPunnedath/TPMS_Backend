using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Disputes.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Disputes.Handlers;

public class DeleteDisputeCommandHandler 
    : IRequestHandler<DeleteDisputeCommand, ApiResponse<bool>>
{
    private readonly TPMSDBContext _context;

    public DeleteDisputeCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(
        DeleteDisputeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var dispute = await _context.Disputes
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (dispute == null)
                return ApiResponse<bool>.Failure("Dispute not found");

            // Optional: prevent double delete
            if (dispute.DeletedAt != null)
                return ApiResponse<bool>.Failure("Dispute already deleted");

            // Soft delete
            dispute.DeletedAt = DateTime.UtcNow;
            dispute.UpdatedByUserId = request.UserId;

            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.Success(true, "Dispute deleted successfully");
        }
        catch (Exception ex)
        {
            // TODO: log ex (ILogger)
            return ApiResponse<bool>.Failure("Error occurred while deleting dispute");
        }
    }
}