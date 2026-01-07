using System.Collections.Generic;

namespace TPMS.Application.Features.Documents.DTOs;

public class DocumentTreeCategoryDto
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<DocumentTreeTypeDto> Types { get; set; } = new();
}