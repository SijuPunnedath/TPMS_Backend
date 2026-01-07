using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Landlords.Commands
{
    public class RestoreLandlordCommand : IRequest<bool>
    {
        public int LandlordId { get; }

        public RestoreLandlordCommand(int landlordId) => LandlordId = landlordId;
    }
}
