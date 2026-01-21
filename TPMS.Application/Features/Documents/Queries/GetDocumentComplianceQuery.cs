using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries;

public class GetDocumentComplianceQuery : IRequest<DocumentComplianceDto>
{
    public int OwnerTypeID { get; }
    public int OwnerID { get; }

    public GetDocumentComplianceQuery(int ownerTypeId, int ownerId)
    {
        OwnerTypeID = ownerTypeId;
        OwnerID = ownerId;
    }
}