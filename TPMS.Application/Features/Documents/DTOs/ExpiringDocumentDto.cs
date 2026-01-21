using System;

namespace TPMS.Application.Features.Documents.DTOs;

public class ExpiringDocumentDto
{
    public int DocumentID { get; set; }
    public string DocumentName { get; set; } = string.Empty;

    public int OwnerTypeID { get; set; }
    public int OwnerID { get; set; }

    public DateTime ValidTo { get; set; }
    public int DaysRemaining { get; set; }
}