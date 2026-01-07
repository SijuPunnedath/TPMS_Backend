using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Properties.Commands
{
    public class RestorePropertyCommand :IRequest<bool> 
    {
        public int PropertyId { get; }
        public RestorePropertyCommand(int propertyId) => PropertyId = propertyId;
    }
}
