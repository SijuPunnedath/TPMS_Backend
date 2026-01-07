using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.RolePermissions.Commands
{
    public record  DeleteRolePermissionCommand(int RolePermissionID) :IRequest<bool>;

}
