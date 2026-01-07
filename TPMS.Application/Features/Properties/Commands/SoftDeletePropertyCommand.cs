using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Properties.Commands
{
    public class SoftDeletePropertyCommand :IRequest<bool>
    {
        public int PropertyId { get; }
        public SoftDeletePropertyCommand(int propertyId) => PropertyId = propertyId;
    }

}
