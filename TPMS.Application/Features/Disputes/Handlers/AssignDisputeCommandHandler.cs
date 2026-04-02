using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Disputes.Commands;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Disputes.Handlers;

public class AssignDisputeCommandHandler 
    : IRequestHandler<AssignDisputeCommand, ApiResponse<bool>>
{
    private readonly TPMSDBContext _context;

    public AssignDisputeCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(
        AssignDisputeCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var dispute = await _context.Disputes
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (dispute == null)
                return ApiResponse<bool>.Failure("Dispute not found");

            // Optional: prevent assigning deleted disputes
            if (dispute.DeletedAt != null)
                return ApiResponse<bool>.Failure("Cannot assign a deleted dispute");

            // Assign
            dispute.AssignedToUserId = request.AssignedToUserId;
            dispute.Status = DisputeStatus.UnderReview;
            dispute.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.Success(true, "Dispute assigned successfully");
        }
        catch (Exception)
        {
            return ApiResponse<bool>.Failure("Error occurred while assigning dispute");
        }
    }
}