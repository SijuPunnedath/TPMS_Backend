using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Domain.Entities
{
    public class DocumentUploadSession
    {
        public Guid SessionId { get; set; }
        public string FileName { get; set; } = string.Empty;

        public int OwnerTypeID { get; set; }
        public int OwnerID { get; set; }

        public string? DocType { get; set; }

        // Added category/type fields to track metadata upfront
        public int? DocumentTypeID { get; set; }
        public int? DocumentCategoryID { get; set; }

        public int TotalChunks { get; set; }
        public int UploadedChunks { get; set; }
        public bool IsCompleted { get; set; } = false;

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public string? Status { get; set; }  // "Pending", "Uploading", "Merging", "Completed", "Failed"
        public string? ErrorMessage { get; set; }

        public int? UploadedBy { get; set; }
        
        public string? Description { get; set; }
        
        public DateTime UpdatedAt { get; set; } 
    }
}
