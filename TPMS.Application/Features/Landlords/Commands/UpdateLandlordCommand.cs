using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.DTOs;

namespace TPMS.Application.Features.Landlords.Commands
{
    public class UpdateLandlordCommand : IRequest<bool>
    {
        public int LandlordId { get; }
        public LandlordDto Landlord { get; }

        public UpdateLandlordCommand(int landlordId, LandlordDto landlord)
        {
            LandlordId = landlordId;
            Landlord = landlord;
        }
    }
}
