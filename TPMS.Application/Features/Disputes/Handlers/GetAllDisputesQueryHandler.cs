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

public class GetAllDisputesQueryHandler 
    : IRequestHandler<GetAllDisputesQuery, ApiResponse<PagedResult<DisputeListDto>>>
{
    private readonly TPMSDBContext _context;

    public GetAllDisputesQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<PagedResult<DisputeListDto>>> Handle(
        GetAllDisputesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.Disputes
                .AsNoTracking()
                .Where(x => x.DeletedAt == null);

            if (request.Status.HasValue)
                query = query.Where(x => x.Status == request.Status.Value);

            if (request.Priority.HasValue)
                query = query.Where(x => x.Priority == request.Priority.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var rawItems = await query
                .OrderByDescending(x => x.RaisedDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new
                {
                    x.DisputeId,
                    x.DisputeNumber,
                    x.Subject,
                    x.Status,
                    x.Priority,
                    x.Category,
                    x.RaisedDate,
                    x.DueDate,
                    x.IsEscalated,
                    RaisedByUser = x.RaisedByUser != null ? x.RaisedByUser.Username : "Unknown",
                    AssignedToUser = x.AssignedToUser != null ? x.AssignedToUser.Username : null
                })
                .ToListAsync(cancellationToken);

            var items = rawItems.Select(x => new DisputeListDto
            {
                DisputeId = x.DisputeId,
                DisputeNumber = x.DisputeNumber,
                Subject = x.Subject,
                Status = x.Status.ToString(),
                Priority = x.Priority.ToString(),
                Category = x.Category.ToString(),
                RaisedDate = x.RaisedDate,
                DueDate = x.DueDate,
                IsEscalated = x.IsEscalated,
                RaisedByUser = x.RaisedByUser,
                AssignedToUser = x.AssignedToUser
            }).ToList();

            var pagedResult = new PagedResult<DisputeListDto>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize
            );

            return ApiResponse<PagedResult<DisputeListDto>>.Success(
                pagedResult,
                "Disputes fetched successfully"
            );
        }
        catch (Exception)
        {
            return ApiResponse<PagedResult<DisputeListDto>>.Failure(
                "Error occurred while fetching disputes"
            );
        }
    }
}