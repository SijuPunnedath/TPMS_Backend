using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Documents.DTOs
{
    public class CompleteUploadDto
    {
        public Guid SessionId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string OwnerType { get; set; } = string.Empty;
        public int OwnerID { get; set; }
        public string? DocType { get; set; }
    }
}
