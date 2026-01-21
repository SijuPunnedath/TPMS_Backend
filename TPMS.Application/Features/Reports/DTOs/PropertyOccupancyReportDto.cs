namespace TPMS.Application.Features.Reports.DTOs;

public class PropertyOccupancyReportDto
{
    public int TotalProperties { get; set; }
    public int OccupiedProperties { get; set; }
    public int VacantProperties { get; set; }

    public decimal OccupancyPercentage =>
        TotalProperties == 0 ? 0 :
            (OccupiedProperties * 100m) / TotalProperties;
}