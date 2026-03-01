using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Properties.Queries;

public class GetPropertyComplianceQuery : IRequest<DocumentHealthDto>
{
    public int PropertyId { get; }

    public GetPropertyComplianceQuery(int propertyId)
    {
        PropertyId = propertyId;
    }
}