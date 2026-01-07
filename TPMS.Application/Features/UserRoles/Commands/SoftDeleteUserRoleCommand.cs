using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.UserRoles.Commands
{
    public record SoftDeleteUserRoleCommand(int UserRoleID) : IRequest<bool>;
}
