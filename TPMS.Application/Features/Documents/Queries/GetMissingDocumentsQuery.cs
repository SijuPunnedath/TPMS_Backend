using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries;

public class GetMissingDocumentsQuery : IRequest<DocumentHealthDto>
{
    public int OwnerTypeID { get; }
    public int OwnerID { get; }

    public GetMissingDocumentsQuery(int ownerTypeId, int ownerId)
    {
        OwnerTypeID = ownerTypeId;
        OwnerID = ownerId;
    }
}