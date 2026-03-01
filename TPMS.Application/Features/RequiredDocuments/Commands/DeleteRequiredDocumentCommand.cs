using MediatR;

namespace TPMS.Application.Features.RequiredDocuments.Commands;

public class DeleteRequiredDocumentCommand : IRequest
{
    public int Id { get; }

    public DeleteRequiredDocumentCommand(int id)
    {
        Id = id;
    }
}
