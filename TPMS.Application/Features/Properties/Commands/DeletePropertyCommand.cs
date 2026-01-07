using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Properties.Commands
{
    public class DeletePropertyCommand :IRequest<bool>
    {
        public int PropertyId { get; }
        public DeletePropertyCommand(int propertyId) => PropertyId = propertyId;
    }

}
