using System.IO;

namespace TPMS.Application.Features.Documents.DTOs;

public class DownloadDocumentResult
{
    public Stream Stream { get; set; } = Stream.Null;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/octet-stream";
}