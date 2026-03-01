using System;

namespace TPMS.Application.Features.Reports.DTOs;

public class DocumentReportDto
{
    public int DocumentID { get; set; }
    public string? DocumentNumber { get; set; }
    public string OwnerType { get; set; } = string.Empty;
    public int OwnerID { get; set; }
    
    public string OwnerName { get; set; } = "";
    public string DocType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string URL { get; set; } = string.Empty;
    public string? Version { get; set; }
    public DateTime UploadedAt { get; set; }
    public string? UploadedByUser { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; } 
}