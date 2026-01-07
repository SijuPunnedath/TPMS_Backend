namespace TPMS.Application.Features.DocumentCategory.DTOs;

public class DocumentTypeNodeDto
{
    public int DocumentTypeID { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}