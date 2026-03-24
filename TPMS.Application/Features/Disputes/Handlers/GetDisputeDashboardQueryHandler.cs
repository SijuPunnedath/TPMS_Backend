using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Disputes.DTOs;
using TPMS.Application.Features.Disputes.Queries;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Disputes.Handlers;

using Microsoft.EntityFrameworkCore;

public class GetDisputeDashboardQueryHandler 
    : IRequestHandler<GetDisputeDashboardQuery, DisputeDashboardDto>
{
    private readonly TPMSDBContext _context;

    public GetDisputeDashboardQueryHandler(TPMSDBContext context)
    {
        _context = context;
    }

   public async Task<DisputeDashboardDto> Handle(
    GetDisputeDashboardQuery request,
    CancellationToken cancellationToken)
{
    var disputes = _context.Disputes
        .AsNoTracking()
        .Where(x => x.DeletedAt == null);

    var total = await disputes.CountAsync(cancellationToken);

    var open = await disputes
        .CountAsync(x => x.Status == DisputeStatus.Open, cancellationToken);

    var inProgress = await disputes
        .CountAsync(x => x.Status == DisputeStatus.InProgress, cancellationToken);

    var closed = await disputes
        .CountAsync(x => x.Status == DisputeStatus.Closed, cancellationToken);

    var escalated = await disputes
        .CountAsync(x => x.IsEscalated, cancellationToken);

    var overdue = await disputes
        .CountAsync(x => x.DueDate < DateTime.UtcNow && x.Status != DisputeStatus.Closed,
            cancellationToken);

    // =============================
    // By Status (FIXED)
    // =============================
    var byStatusRaw = await disputes
        .GroupBy(x => x.Status)
        .Select(g => new
        {
            Status = g.Key,
            Count = g.Count()
        })
        .ToListAsync(cancellationToken);

    var byStatus = byStatusRaw
        .Select(x => new ChartItemDto
        {
            Label = x.Status.ToString(),
            Count = x.Count
        })
        .ToList();

    // =============================
    // By Category (FIXED)
    // =============================
    var byCategoryRaw = await disputes
        .GroupBy(x => x.Category)
        .Select(g => new
        {
            Category = g.Key,
            Count = g.Count()
        })
        .ToListAsync(cancellationToken);

    var byCategory = byCategoryRaw
        .Select(x => new ChartItemDto
        {
            Label = x.Category.ToString(),
            Count = x.Count
        })
        .ToList();

    // =============================
    // By Priority (FIXED)
    // =============================
    var byPriorityRaw = await disputes
        .GroupBy(x => x.Priority)
        .Select(g => new
        {
            Priority = g.Key,
            Count = g.Count()
        })
        .ToListAsync(cancellationToken);

    var byPriority = byPriorityRaw
        .Select(x => new ChartItemDto
        {
            Label = x.Priority.ToString(),
            Count = x.Count
        })
        .ToList();

    // =============================
    // Monthly Trend (FIXED)
    // =============================
    var startDate = DateTime.UtcNow.AddMonths(-12);

    var monthlyRaw = await disputes
        .Where(x => x.RaisedDate >= startDate)
        .GroupBy(x => new { x.RaisedDate.Year, x.RaisedDate.Month })
        .Select(g => new
        {
            g.Key.Year,
            g.Key.Month,
            Count = g.Count()
        })
        .OrderBy(x => x.Year)
        .ThenBy(x => x.Month)
        .ToListAsync(cancellationToken);

    var monthlyTrend = monthlyRaw
        .Select(x => new MonthlyTrendDto
        {
            Month = $"{x.Year}-{x.Month:D2}", // safe formatting
            Count = x.Count
        })
        .ToList();

    // =============================
    // SLA Metrics (UNCHANGED)
    // =============================
    var resolvedDisputes = await disputes
        .Where(x => x.ClosedAt != null)
        .ToListAsync(cancellationToken);

    double avgResolutionDays = 0;
    double closedWithinSla = 0;

    if (resolvedDisputes.Any())
    {
        avgResolutionDays = resolvedDisputes
            .Average(x => (x.ClosedAt.Value - x.RaisedDate).TotalDays);

        closedWithinSla = resolvedDisputes
            .Count(x => x.DueDate != null && x.ClosedAt <= x.DueDate)
            * 100.0 / resolvedDisputes.Count;
    }

    return new DisputeDashboardDto
    {
        Summary = new DisputeSummaryDto
        {
            Total = total,
            Open = open,
            InProgress = inProgress,
            Closed = closed,
            Escalated = escalated,
            // Overdue = overdue
        },
        ByStatus = byStatus,
        ByCategory = byCategory,
        ByPriority = byPriority,
        MonthlyTrend = monthlyTrend,
        SlaMetrics = new SlaMetricsDto
        {
            AverageResolutionDays = avgResolutionDays,
            ClosedWithinSlaPercentage = closedWithinSla
        }
    };
}
}