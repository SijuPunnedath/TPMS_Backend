using MediatR;

namespace TPMS.Application.Features.DocumentSequences.Commands;

public class CreateDocumentSequenceCommand : IRequest<int>
{
   // public int TenantId { get; set; }
    public string ModuleName { get; set; } = default!;
    public string Prefix { get; set; } = default!;
    public int NumberLength { get; set; } = 5;
    public bool ResetEveryYear { get; set; }
}
