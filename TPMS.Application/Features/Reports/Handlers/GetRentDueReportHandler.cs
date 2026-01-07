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

public class GetRentDueReportHandler 
    : IRequestHandler<GetRentDueReportQuery, PagedResult<RentDueReportDto>>
{
    private readonly TPMSDBContext _db;
    public GetRentDueReportHandler(TPMSDBContext db) => _db = db;

    public async Task<PagedResult<RentDueReportDto>> Handle(
        GetRentDueReportQuery request, 
        CancellationToken cancellationToken)
    {
        var query = _db.RentSchedules
            .Where(rs => !rs.IsPaid)   // Only pending
            .OrderBy(rs => rs.DueDate)
            .AsQueryable();

        // Paging
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(rs => new RentDueReportDto
            {
                ScheduleID = rs.ScheduleID,
                LeaseID = rs.LeaseID,
                TenantName = rs.Lease.Tenant.Name,
                DueDate = rs.DueDate,
                Amount = rs.Amount,
                IsPaid = rs.IsPaid
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<RentDueReportDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize
        );
    }
}
