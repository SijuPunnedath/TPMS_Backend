using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Documents.DTOs
{
    public class UploadChunkDto
    {
        public Guid SessionId { get; set; }            // Unique upload session (client or server generated)
        public int ChunkNumber { get; set; }           // 1-based index
        public int TotalChunks { get; set; }
        public string FileName { get; set; } = string.Empty;

        public string OwnerType { get; set; } = string.Empty;
        public int OwnerID { get; set; }

        // New fields for category/type metadata (validate during finalize)
        public int? DocumentTypeID { get; set; }
        public int? DocumentCategoryID { get; set; }

        public string? DocType { get; set; }           // optional human-friendly type name
        public int? UploadedBy { get; set; }
        public string? Description { get; set; }

        [Required]
        public IFormFile File { get; set; } = default!; // current chunk bytes
    }
}
