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

public class GetOverduePaymentsReportHandler 
    : IRequestHandler<GetOverduePaymentsReportQuery, PagedResult<OverduePaymentsReportDto>>
{
    private readonly TPMSDBContext _db;
    public GetOverduePaymentsReportHandler(TPMSDBContext db) => _db = db;

    public async Task<PagedResult<OverduePaymentsReportDto>> Handle(
        GetOverduePaymentsReportQuery request, 
        CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var query = _db.RentSchedules
            .Where(rs => !rs.IsPaid && rs.DueDate < today)
            .OrderBy(rs => rs.DueDate)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(rs => new OverduePaymentsReportDto
            {
                ScheduleID = rs.ScheduleID,
                LeaseID = rs.LeaseID,
                TenantName = rs.Lease.Tenant.Name,
                DueDate = rs.DueDate,
                DaysLate = (today - rs.DueDate).Days,  // FIXED
                Amount = rs.Amount,
                Penalty = rs.Penalty
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<OverduePaymentsReportDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize
        );
    }
}
