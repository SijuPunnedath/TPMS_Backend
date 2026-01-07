

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Reports.DTOs;
using TPMS.Application.Features.Reports.Queries;
using TPMS.Infrastructure.Persistence.Configurations;
namespace TPMS.Application.Features.Reports.Handlers;
public class GetLeaseExpiryReportHandler 
    : IRequestHandler<GetLeaseExpiryReportQuery, PagedResult<LeaseExpiryReportDto>>
{
    private readonly TPMSDBContext _db;

    public GetLeaseExpiryReportHandler(TPMSDBContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<LeaseExpiryReportDto>> Handle(
        GetLeaseExpiryReportQuery request, 
        CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var query = _db.Leases
            .Include(l => l.Tenant)
            .Include(l => l.Landlord)
            .Where(l => !l.IsDeleted)
            .AsQueryable();

        // --- Apply Filters ---
        if (request.FromDate.HasValue)
            query = query.Where(l => l.EndDate >= request.FromDate.Value.Date);

        if (request.ToDate.HasValue)
            query = query.Where(l => l.EndDate <= request.ToDate.Value.Date);

        if (request.TenantId.HasValue)
            query = query.Where(l => l.TenantID == request.TenantId.Value);

        if (request.LandlordId.HasValue)
            query = query.Where(l => l.LandlordID == request.LandlordId.Value);

        // Total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Paging
        var leases = await query
            .OrderBy(l => l.EndDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // Map to DTO
        var result = leases.Select(l => new LeaseExpiryReportDto
        {
            LeaseID = l.LeaseID,
            TenantName = l.Tenant?.Name ?? "",
            LandlordName = l.Landlord?.Name ?? "",
            StartDate = l.StartDate,
            EndDate = l.EndDate,
            DaysRemaining = (l.EndDate - today).Days,
            Rent = l.Rent
        }).ToList();

        return new PagedResult<LeaseExpiryReportDto>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize
        );
    }
}
