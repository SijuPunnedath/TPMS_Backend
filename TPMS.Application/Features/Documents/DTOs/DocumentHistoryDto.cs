using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Documents.DTOs
{
    public class DocumentHistoryDto
    {
        public int DocumentID { get; set; }
        public string? DocType { get; set; }
        public string? FileName { get; set; }
        public string? URL { get; set; }
        public string? Version { get; set; }
        public string? UploadedByName { get; set; }
        public DateTime? UploadedAt { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}
