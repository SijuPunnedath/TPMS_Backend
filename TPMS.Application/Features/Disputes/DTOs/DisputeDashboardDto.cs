using System.Collections.Generic;

namespace TPMS.Application.Features.Disputes.DTOs;

public class DisputeDashboardDto
{
    public DisputeSummaryDto Summary { get; set; }
    public List<ChartItemDto> ByStatus { get; set; }
    public List<ChartItemDto> ByCategory { get; set; }
    public List<ChartItemDto> ByPriority { get; set; }
    public List<MonthlyTrendDto> MonthlyTrend { get; set; }
    public SlaMetricsDto SlaMetrics { get; set; }
}