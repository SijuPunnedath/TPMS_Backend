using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Disputes.Commands;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Disputes.Handlers;

public class CreateDisputeCommandHandler 
    : IRequestHandler<CreateDisputeCommand, ApiResponse<int>>
{
    private readonly TPMSDBContext _context;

    public CreateDisputeCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<int>> Handle(
        CreateDisputeCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var dispute = new Dispute
            {
                DisputeNumber = string.IsNullOrWhiteSpace(request.DisputeNumber)
                    ? GenerateDisputeNumber(request.TenantId)
                    : request.DisputeNumber,

                TenantId = request.TenantId,
                RaisedByUserId = request.RaisedByUserId,
                RaisedBy = request.RaisedBy,
                Category = request.Category,
                Status = DisputeStatus.Open,
                Priority = request.Priority,
                Subject = request.Subject,
                Description = request.Description,
                ReferenceType = request.ReferenceType,
                ReferenceId = request.ReferenceId,
                RaisedDate = DateTime.UtcNow,
                DueDate = request.DueDate,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = request.RaisedByUserId
            };

            _context.Disputes.Add(dispute);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<int>.Success(
                dispute.DisputeId,
                "Dispute created successfully"
            );
        }
        catch (Exception ex)
        {
            // TODO: log ex using ILogger
            return ApiResponse<int>.Failure(
                "Error occurred while creating dispute"
            );
        }
    }

    private string GenerateDisputeNumber(int tenantId)
    {
        var year = DateTime.UtcNow.Year;
        var count = _context.Disputes
            .Count(d => d.TenantId == tenantId) + 1;

        return $"DSP-{year}-{count:D4}";
    }
}