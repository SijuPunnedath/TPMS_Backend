using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Disputes.DTOs;
using TPMS.Application.Features.Disputes.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Disputes.Handlers;

public class GetDisputeByIdQueryHandler 
    : IRequestHandler<GetDisputeByIdQuery, ApiResponse<DisputeDto>>
{
    private readonly TPMSDBContext _context;

    public GetDisputeByIdQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<DisputeDto>> Handle(
        GetDisputeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var dispute = await _context.Disputes
                .AsNoTracking()
                .Where(d => d.DisputeId == request.Id && d.DeletedAt == null)
                .Select(d => new DisputeDto
                {
                    DisputeId = d.DisputeId,
                    DisputeNumber = d.DisputeNumber,
                    TenantId = d.TenantId,
                    Category = d.Category,
                    Status = d.Status,
                    Priority = d.Priority,
                    Subject = d.Subject,
                    Description = d.Description,
                    RaisedDate = d.RaisedDate,
                    DueDate = d.DueDate,
                    AssignedToUserId = d.AssignedToUserId,
                    IsEscalated = d.IsEscalated,
                    ClosedAt = d.ClosedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (dispute == null)
                return ApiResponse<DisputeDto>.Failure("Dispute not found");

            return ApiResponse<DisputeDto>.Success(
                dispute,
                "Dispute fetched successfully"
            );
        }
        catch (Exception)
        {
            return ApiResponse<DisputeDto>.Failure(
                "Error occurred while fetching dispute"
            );
        }
    }
}