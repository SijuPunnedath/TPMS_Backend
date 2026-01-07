using System.Collections.Generic;

namespace TPMS.Application.Features.Documents.DTOs;

public class DocumentTreeTypeDto
{
    public int DocumentTypeID { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public List<DocumentTreeDocumentDto> Documents { get; set; } = new();
}