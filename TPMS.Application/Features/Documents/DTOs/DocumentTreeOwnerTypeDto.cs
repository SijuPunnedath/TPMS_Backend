using System.Collections.Generic;

namespace TPMS.Application.Features.Documents.DTOs;

public class DocumentTreeOwnerTypeDto
{
    public string OwnerType { get; set; } = string.Empty;
    public List<DocumentTreeOwnerDto> Owners { get; set; } = new();
}