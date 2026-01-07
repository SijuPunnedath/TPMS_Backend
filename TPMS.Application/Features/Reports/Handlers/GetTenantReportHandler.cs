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

public class GetTenantReportHandler : IRequestHandler<GetTenantReportQuery, PagedResult<TenantReportDto>>
{
  private readonly TPMSDBContext _db;
    public GetTenantReportHandler(TPMSDBContext db) => _db = db;

    public async Task<PagedResult<TenantReportDto>> Handle(GetTenantReportQuery request, CancellationToken cancellationToken)
    {
        var q = _db.Tenants.AsNoTracking().Where(t => !t.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.Trim().ToLower();
            q = q.Where(t => (t.Name ?? "").ToLower().Contains(s));
        }

        var tenantList = await q.ToListAsync(cancellationToken);
        var tenantIds = tenantList.Select(t => t.TenantID).ToList();

        var ownerTypeId = await _db.OwnerTypes
            .Where(o => o.Name == "Tenant")
            .Select(o => o.OwnerTypeID)
            .FirstAsync(cancellationToken);

        var addresses = await _db.Addresses
            .Where(a => a.OwnerTypeID == ownerTypeId && a.IsPrimary)
            .ToListAsync(cancellationToken);

        var outstanding = await _db.RentSchedules
            .Where(rs => !rs.IsPaid)
            .GroupBy(rs => rs.Lease.TenantID)
            .Select(g => new { g.Key, Amount = g.Sum(x => x.Amount + (x.Penalty ?? 0)) })
            .ToListAsync(cancellationToken);

        var activeLeaseCounts = await _db.Leases
            .GroupBy(l => l.TenantID)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var result = tenantList.Select(t =>
        {
            var addr = addresses.FirstOrDefault(a => a.OwnerID == t.TenantID);
            var outg = outstanding.FirstOrDefault(x => x.Key == t.TenantID);
            var leases = activeLeaseCounts.FirstOrDefault(x => x.Key == t.TenantID);

            return new TenantReportDto
            {
                TenantID = t.TenantID,
                Name = t.Name,
                PrimaryEmail = addr?.Email,
                PrimaryPhone = addr?.Phone1,
                IsDeleted = t.IsDeleted,
                ActiveLeaseCount = leases?.Count ?? 0,
                OutstandingAmount = outg?.Amount ?? 0
            };
        }).ToList();

        // Pagination
        var total = result.Count;
        var pagedItems = result
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PagedResult<TenantReportDto>(
            pagedItems,
            total,
            request.PageNumber,
            request.PageSize
        );
    }
    
}