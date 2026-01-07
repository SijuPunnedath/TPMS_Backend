namespace TPMS.Application.Features.Reports.DTOs;

public class TenantReportDto
{
    public int TenantID { get; set; }
    public string? Name { get; set; }
    public string? PrimaryEmail { get; set; }
    public string? PrimaryPhone { get; set; }
    public bool IsDeleted { get; set; }
    public int ActiveLeaseCount { get; set; }
    public decimal OutstandingAmount { get; set; }
}