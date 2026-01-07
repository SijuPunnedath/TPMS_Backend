using System.IO;

namespace TPMS.Application.Features.Documents.DTOs;

public class ViewDocumentResult
{
    public Stream Stream { get; set; } = null!;
    public string ContentType { get; set; } = string.Empty;
}
