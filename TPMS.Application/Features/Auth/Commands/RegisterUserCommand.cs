using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Auth.Commands
{
    using MediatR;
    using TPMS.Application.Features.Auth.DTOs;

    public record RegisterUserCommand(RegisterUserDto Dto) : IRequest<int>;
}
