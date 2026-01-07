using System;

namespace TPMS.Application.Features.Documents.DTOs;

public class DocumentUploadSessionDto
{
    public Guid SessionId { get; set; }
    public string FileName { get; set; } = string.Empty;

    public string OwnerType { get; set; } = string.Empty;
    public int OwnerID { get; set; }

    public int TotalChunks { get; set; }
    public int UploadedChunks { get; set; }

    public bool IsCompleted { get; set; }
    public string? Status { get; set; }      // Pending, Uploading, Completed, Failed
    public string? ErrorMessage { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public double ProgressPercentage =>
        TotalChunks == 0 ? 0 : Math.Round((double)UploadedChunks / TotalChunks * 100, 2);
}