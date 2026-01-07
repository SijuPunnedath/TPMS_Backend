using System.Collections.Generic;

namespace TPMS.Application.Features.Documents.DTOs;

public class DocumentTreeOwnerDto
{
    public int OwnerID { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public List<DocumentTreeCategoryDto> Categories { get; set; } = new();
}