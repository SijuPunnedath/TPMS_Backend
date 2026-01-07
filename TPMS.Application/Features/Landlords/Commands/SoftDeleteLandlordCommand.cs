using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Landlords.Commands
{
    public class SoftDeleteLandlordCommand : IRequest<bool>
    {
        public int LandlordId { get; }

        public SoftDeleteLandlordCommand(int landlordId)
        {
            LandlordId = landlordId;
        }
    }
}
