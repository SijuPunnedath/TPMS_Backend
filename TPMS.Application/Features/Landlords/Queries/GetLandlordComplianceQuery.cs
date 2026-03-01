using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Landlords.Queries;

public class GetLandlordComplianceQuery : IRequest<DocumentHealthDto>
{
    public int LandlordId { get; }

    public GetLandlordComplianceQuery(int landlordId)
    {
        LandlordId = landlordId;
    }
}