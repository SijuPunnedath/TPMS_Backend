using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Documents.DTOs
{
    public class DownloadDocumentDto
    {
        public int DocumentID { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = "application/octet-stream";
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
        public string? Version { get; set; }
        public string? URL { get; set; } // For cloud download redirect
    }
}
