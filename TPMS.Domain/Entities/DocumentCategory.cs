namespace TPMS.Domain.Entities;

public class DocumentCategory
{
    public int DocumentCategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty; // Property Docs, Lease Docs
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    
    public bool IsDeleted { get; set; } = false;
    
    public DateTime CreatedDate { get; set; } 
    public DateTime UpdatedAt { get; set; } 
    

    public virtual ICollection<DocumentType> DocumentTypes { get; set; } = new List<DocumentType>();
}
