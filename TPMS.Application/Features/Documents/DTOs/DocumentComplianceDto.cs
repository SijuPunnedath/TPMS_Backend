namespace TPMS.Application.Features.Documents.DTOs;

public class DocumentComplianceDto
{
    public int OwnerTypeID { get; set; }
    public int OwnerID { get; set; }

    public int TotalRequired { get; set; }
    public int Uploaded { get; set; }

    public decimal CompliancePercentage { get; set; }
}