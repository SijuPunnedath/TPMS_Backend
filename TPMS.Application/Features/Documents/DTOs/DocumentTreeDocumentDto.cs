namespace TPMS.Application.Features.Documents.DTOs;

public class DocumentTreeDocumentDto
{
    public int DocumentID { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string URL { get; set; } = string.Empty;
    public string? Version { get; set; }
}