using MediatR;

namespace TPMS.Application.Features.DocumentSequences.Commands;

public class UpdateDocumentSequenceCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Prefix { get; set; } = default!;
    public int NumberLength { get; set; }
    public bool ResetEveryYear { get; set; }
}
