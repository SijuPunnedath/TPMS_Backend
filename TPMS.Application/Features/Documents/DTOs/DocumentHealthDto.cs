using System.Collections.Generic;

namespace TPMS.Application.Features.Documents.DTOs;

public class DocumentHealthDto
{
    public int OwnerTypeID { get; set; }
    public int OwnerID { get; set; }

    public int TotalRequired { get; set; }
    public int Uploaded { get; set; }
    public int Missing { get; set; }

    public List<MissingDocumentDto> MissingDocuments { get; set; } = new();
}