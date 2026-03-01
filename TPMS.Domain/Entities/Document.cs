using System;

namespace TPMS.Domain.Entities
{
    public class Document
    {
       
        public int DocumentID { get; set; }

        public string DocumentName { get; set; }
        // Relationship to master OwnerType (Tenant, Landlord, Property, Lease, Common)
        public int OwnerTypeID { get; set; }

        // The specific record ID (like TenantID, LeaseID, etc.)
        public int OwnerID { get; set; }

        public int DocumentTypeID { get; set; }

        // Document category/type (e.g., "Lease Agreement", "KYC", "Invoice")
        public string? DocType { get; set; }

        public int DocumentCategoryID { get; set; }
        
        public string? DocumentCategoryName { get; set; }
        // The original file name
        public string? FileName { get; set; }

        // The file storage path or public URL (IIS folder or cloud storage)
        public string? URL { get; set; }

        // User ID who uploaded
        public int? UploadedBy { get; set; }

        // Audit trail
        public DateTime UploadedAt { get; set; } 

        // Version for tracking updated files
        public string? Version { get; set; }

       // New fields for version control
        public bool IsArchived { get; set; } = false;
       
        public int? PreviousDocumentID { get; set; }

        public string? DocumentNumber { get; set; }

        // Optional description or notes
        public string? Description { get; set; }

        // Navigation
        public virtual OwnerType? OwnerType { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
        
        public virtual DocumentType? DocumentType { get; set; }
        
        public virtual DocumentCategory? DocumentCategory => DocumentType?.Category;
        
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }   // Expiry date (NOC, Lease, Insurance)
    }
}
