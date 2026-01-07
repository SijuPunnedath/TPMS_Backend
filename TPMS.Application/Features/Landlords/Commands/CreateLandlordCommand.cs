using MediatR;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Domain.Entities;

namespace TPMS.Application.Features.Landlords.Commands
{
    public class CreateLandlordCommand : IRequest<int>
    {
        public CreateLandlordDto Landlord { get; }
        public CreateLandlordCommand(CreateLandlordDto landlord) => Landlord = landlord;
    }
}
