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

public class GetRentPaymentHistoryHandler 
    : IRequestHandler<GetRentPaymentHistoryQuery, PagedResult<RentPaymentHistoryDto>>
{
    private readonly TPMSDBContext _db;
    public GetRentPaymentHistoryHandler(TPMSDBContext db) => _db = db;

    public async Task<PagedResult<RentPaymentHistoryDto>> Handle(
        GetRentPaymentHistoryQuery request, 
        CancellationToken cancellationToken)
    {
        var query = _db.RentSchedules
            .Where(rs => rs.IsPaid)   // Only paid schedules
            .OrderByDescending(rs => rs.PaidDate)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(rs => new RentPaymentHistoryDto
            {
                ScheduleID = rs.ScheduleID,
                LeaseID = rs.LeaseID,
                TenantName = rs.Lease.Tenant.Name,
                PaidDate = rs.PaidDate.Value,
                Amount = rs.Amount,
                Penalty = rs.Penalty
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<RentPaymentHistoryDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize
        );
    }
}
