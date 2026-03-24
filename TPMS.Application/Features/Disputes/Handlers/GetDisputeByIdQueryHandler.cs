using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Exceptions;
using TPMS.Application.Features.Disputes.DTOs;
using TPMS.Application.Features.Disputes.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Disputes.Handlers;

public class GetDisputeByIdQueryHandler 
    : IRequestHandler<GetDisputeByIdQuery, DisputeDto>
{
    private readonly TPMSDBContext _context;

    public GetDisputeByIdQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<DisputeDto> Handle(
        GetDisputeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var dispute = await _context.Disputes
            .Where(d => d.DisputeId == request.Id)
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
        {
            throw new Exception($"Dispute with Id {request.Id} not found");
            //  Replace with NotFoundException if you have one
        }

        return dispute;
    }
}