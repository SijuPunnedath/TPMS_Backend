namespace TPMS.Application.Features.DocumentSequences.DTOs;

public class DocumentSequenceDto
{
    public int Id { get; set; }
    public string ModuleName { get; set; } = default!;
    public string Prefix { get; set; } = default!;
    public int CurrentNumber { get; set; }
    public int NumberLength { get; set; }
    public bool ResetEveryYear { get; set; }
    public int? Year { get; set; }
}
