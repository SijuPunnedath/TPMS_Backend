namespace TPMS.Application.Features.RequiredDocuments.DTOs;

public class RequiredDocumentDto
{
    public int RequiredDocumentID { get; set; }
    public int OwnerTypeID { get; set; }
    public string OwnerTypeName { get; set; }

    public int DocumentTypeID { get; set; }
    public string DocumentTypeName { get; set; }

    public bool IsMandatory { get; set; }
    public bool IsActive { get; set; }
}
