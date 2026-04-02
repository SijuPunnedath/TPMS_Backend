using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Disputes.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Disputes.Handlers;

public class UpdateDisputeCommandHandler 
    : IRequestHandler<UpdateDisputeCommand, ApiResponse<bool>>
{
    private readonly TPMSDBContext _context;

    public UpdateDisputeCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(
        UpdateDisputeCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var dispute = await _context.Disputes
                .FirstOrDefaultAsync(
                    x => x.DisputeId == request.Id && x.DeletedAt == null,
                    cancellationToken);

            if (dispute == null)
                return ApiResponse<bool>.Failure("Dispute not found");

            // Update fields
            dispute.Category = request.Category;
            dispute.Priority = request.Priority;
            dispute.Subject = request.Subject;
            dispute.Description = request.Description;
            dispute.DueDate = request.DueDate;
            dispute.UpdatedAt = DateTime.UtcNow;
            dispute.UpdatedByUserId = request.UpdatedByUserId;

            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.Success(true, "Dispute updated successfully");
        }
        catch (Exception)
        {
            return ApiResponse<bool>.Failure("Error occurred while updating dispute");
        }
    }
}