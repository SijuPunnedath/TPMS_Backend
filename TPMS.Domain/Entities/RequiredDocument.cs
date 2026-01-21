namespace TPMS.Domain.Entities;

public class RequiredDocument
{
    public int RequiredDocumentID { get; set; }

    public int OwnerTypeID { get; set; }       // Tenant / Lease / Asset
    public int DocumentTypeID { get; set; }    // FK to DocumentType

    public bool IsMandatory { get; set; } = true;
    
    public bool IsActive { get; set; } = true;

    public virtual DocumentType? DocumentType { get; set; }
    public virtual OwnerType? OwnerType { get; set; }
}
