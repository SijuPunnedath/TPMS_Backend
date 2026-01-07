using MediatR;
using TPMS.Application.Features.Landlords.DTOs;

namespace TPMS.Application.Features.Landlords.Queries
{
    public class GetLandlordByIdQuery : IRequest<LandlordDto>
    {
        public int LandlordId { get; }
        public GetLandlordByIdQuery(int landlordId) => LandlordId = landlordId;
    }
}
