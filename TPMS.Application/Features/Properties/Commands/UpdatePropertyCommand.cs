using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Properties.DTOs;

namespace TPMS.Application.Features.Properties.Commands
{
    public class UpdatePropertyCommand : IRequest<bool>
    {
        public int PropertyId { get; }
        public PropertyDto Property { get; }

        public UpdatePropertyCommand(int propertyId, PropertyDto property)
        {
            PropertyId = propertyId;
            Property = property;
        }
    }
}
