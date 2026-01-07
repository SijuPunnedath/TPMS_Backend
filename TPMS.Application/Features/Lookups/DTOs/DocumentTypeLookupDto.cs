namespace TPMS.Application.Features.Lookups.DTOs;

public class DocumentTypeLookupDto
{
    public int DocumentTypeID { get; set; }
    public int DocumentCategoryID { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
}