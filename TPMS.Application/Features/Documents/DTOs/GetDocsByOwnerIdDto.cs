using System;

namespace TPMS.Application.Features.Documents.DTOs;

public class GetDocsByOwnerIdDto
{
    public int DocumentID { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }

    public int OwnerTypeID { get; set; }
    public int OwnerID { get; set; }

    public int DocumentTypeID { get; set; }
    public string DocumentTypeName { get; set; } = string.Empty;

    public int DocumentCategoryID { get; set; }
    public string DocumentCategoryName { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }
}