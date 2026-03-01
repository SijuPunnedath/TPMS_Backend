
using System;
using Microsoft.AspNetCore.Http;

namespace TPMS.Application.Features.Documents.DTOs
{
    public class UploadDocumentDto
    {
        public string OwnerType { get; set; } = string.Empty;   // e.g., "Lease", "Tenant", "Landlord"
        public int OwnerID { get; set; }
        public int? DocumentTypeID { get; set; }
        public string? DocumentTypeName { get; set; }
        
        public int DocumentCategoryID { get; set; }
        public string? DocumentCategoryName { get; set; }
        public string? DocType { get; set; }
        public int? UploadedBy { get; set; }
        public IFormFile File { get; set; } = null!;
        public string? Description { get; set; }
        public string? Version { get; set; }
       
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }  
        
       

    }
}
