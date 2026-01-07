using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Users.Commands
{
    public record SoftDeleteUserCommand(int UserId) : IRequest<bool>;
}
