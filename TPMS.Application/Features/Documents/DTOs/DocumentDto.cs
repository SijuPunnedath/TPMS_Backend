using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Documents.DTOs
{
    public class DocumentDto
    {
        public int DocumentID { get; set; }
        
        public string? DocumentNumber { get; set; }
        public string DocumentName { get; set; }
        public int OwnerTypeID { get; set; }
        public int OwnerID { get; set; }
        public int DocumentTypeID { get; set; }
        public string? DocumentTypeName { get; set; }
        public int DocumentCategoryID { get; set; }
        public string? DocumentCategoryName { get; set; }
        public string? DocType { get; set; }
        public string? FileName { get; set; }
        public string? URL { get; set; }
        public int? UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }
        public string? Version { get; set; }
        public string? Description { get; set; }
        // New fields for version control
        public bool IsArchived { get; set; } = false;
        public int? PreviousDocumentID { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
