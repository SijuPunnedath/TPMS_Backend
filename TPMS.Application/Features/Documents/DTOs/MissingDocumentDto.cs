namespace TPMS.Application.Features.Documents.DTOs;

public class MissingDocumentDto
{
    public int DocumentTypeID { get; set; }
    
    public string DocumentTypeName { get; set; } = string.Empty;
    
    public string CategoryName { get; set; } = string.Empty;
   
    public int CategoryId { get; set; }
    public bool IsMandatory { get; set; }
}