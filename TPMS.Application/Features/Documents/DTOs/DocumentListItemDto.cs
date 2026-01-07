using System;

namespace TPMS.Application.Features.Documents.DTOs;

public class DocumentListItemDto
{
    public int DocumentID { get; set; }
    public string DocumentName { get; set; } = string.Empty;

    public int DocumentTypeID { get; set; }
    public string DocumentTypeName { get; set; } = string.Empty;

    public int DocumentCategoryID { get; set; }
    public string DocumentCategoryName { get; set; } = string.Empty;

    public string? Version { get; set; }
    public bool IsActive { get; set; }

    public DateTime UploadedAt { get; set; }
    public int? UploadedBy { get; set; }

    public string? FileName { get; set; }
}