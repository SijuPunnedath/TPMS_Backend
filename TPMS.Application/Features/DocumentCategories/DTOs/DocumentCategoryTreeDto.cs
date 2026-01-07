using System.Collections.Generic;

namespace TPMS.Application.Features.DocumentCategory.DTOs;

public class DocumentCategoryTreeDto
{
    public int DocumentCategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public List<DocumentTypeNodeDto> Types { get; set; } = new();
}