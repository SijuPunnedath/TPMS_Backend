using MediatR;

namespace TPMS.Application.Features.DocumentSequences.Commands;

public class ResetDocumentSequenceCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
