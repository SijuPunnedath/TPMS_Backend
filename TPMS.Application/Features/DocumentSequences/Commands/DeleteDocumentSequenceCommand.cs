using MediatR;

namespace TPMS.Application.Features.DocumentSequences.Commands;

public class DeleteDocumentSequenceCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
