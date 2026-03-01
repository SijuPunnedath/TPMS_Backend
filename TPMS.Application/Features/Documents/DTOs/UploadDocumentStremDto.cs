using System;
using Microsoft.AspNetCore.Http;

namespace TPMS.Application.Features.Documents.DTOs;

public class UploadDocumentStremDto
{
    // REQUIRED – resolved server-side to OwnerTypeID
    public string OwnerType { get; set; } = string.Empty;
    public int OwnerID { get; set; }

    // Document classification
    public int? DocumentTypeID { get; set; }
    public string? DocType { get; set; }

    public int DocumentCategoryID { get; set; }

    // Metadata
    public string DocumentName { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public int? UploadedBy { get; set; }
    public string? Description { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }  

    // REQUIRED
    public IFormFile File { get; set; } = null!;
}