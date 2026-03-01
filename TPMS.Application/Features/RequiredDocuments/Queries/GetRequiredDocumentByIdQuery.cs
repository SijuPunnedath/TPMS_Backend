using MediatR;
using TPMS.Application.Features.RequiredDocuments.DTOs;

namespace TPMS.Application.Features.RequiredDocuments.Queries;

public class GetRequiredDocumentByIdQuery : IRequest<RequiredDocumentDto>
{
    public int Id { get; }

    public GetRequiredDocumentByIdQuery(int id)
    {
        Id = id;
    }
}
