using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Documents.DTOs
{
    public class DocumentAccessLogDto
    {
        public int LogID { get; set; }
        public int DocumentID { get; set; }
        public string? FileName { get; set; }
        public string? DocType { get; set; }
        public int DocumentTypeID { get; set; }
        public string DocumentCategoryName { get; set; }
        public int DocumentCategoryID { get; set; }
        public string? AccessedBy { get; set; }
        public DateTime AccessedAt { get; set; }
        public string? AccessType { get; set; }
        public string? IPAddress { get; set; }
        public string? Device { get; set; }
        public string? Notes { get; set; }
    }
}
