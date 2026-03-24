

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

public class GetAllDisputesQueryHandler 
    : IRequestHandler<GetAllDisputesQuery, PagedResult<DisputeListDto>>
{
    private readonly TPMSDBContext _context;

    public GetAllDisputesQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<DisputeListDto>> Handle(
        GetAllDisputesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {

        // Base Query (NO Include needed)
        var query = _context.Disputes
            .AsNoTracking()
            .Where(x => x.DeletedAt == null);

        // Filters
        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        if (request.Priority.HasValue)
            query = query.Where(x => x.Priority == request.Priority.Value);

        // Total Count
        var totalCount = await query.CountAsync(cancellationToken);

        // Fetch RAW data (NO ToString here)
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

                // SAFE NULL HANDLING
                RaisedByUser = x.RaisedByUser != null 
                    ? x.RaisedByUser.Username 
                    : "Unknown",

                AssignedToUser = x.AssignedToUser != null
                    ? x.AssignedToUser.Username
                    : null
            })
            .ToListAsync(cancellationToken);

        // Map to DTO (IN MEMORY → safe for ToString)
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

        return new PagedResult<DisputeListDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize
        );

       
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

/*
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

public class GetAllDisputesQueryHandler 
    : IRequestHandler<GetAllDisputesQuery, PagedResult<DisputeListDto>>
{
    private readonly TPMSDBContext _context;

    public GetAllDisputesQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<DisputeListDto>> Handle(
        GetAllDisputesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Disputes
            .AsNoTracking()
            .Include(x => x.RaisedByUser)
            .Include(x => x.AssignedToUser)
            .Where(x => x.TenantId == request.TenantId && x.DeletedAt == null);

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status);

        if (request.Priority.HasValue)
            query = query.Where(x => x.Priority == request.Priority);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.RaisedDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new DisputeListDto
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
                RaisedByUser = x.RaisedByUser.Username,
                AssignedToUser = x.AssignedToUser != null
                    ? x.AssignedToUser.Username
                    : null
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<DisputeListDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize
        );
    }
}*/