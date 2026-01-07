using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Landlords.DTOs;

namespace TPMS.Application.Features.Landlords.Queries
{
    public class GetDeletedLandlordsQuery : IRequest<IEnumerable<LandlordDto>>
    {
       
    }
}
