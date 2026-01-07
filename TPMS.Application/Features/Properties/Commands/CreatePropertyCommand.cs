using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Properties.DTOs;

namespace TPMS.Application.Features.Properties.Commands
{
    public record CreatePropertyCommand(CreatePropertyDto Dto) : IRequest<PropertyDto>;

}
