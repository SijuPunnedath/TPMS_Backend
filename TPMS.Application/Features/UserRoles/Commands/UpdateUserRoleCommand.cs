using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.UserRoles.DTOs;

namespace TPMS.Application.Features.UserRoles.Commands
{
    public record UpdateUserRoleCommand(
    int UserRoleID,
    int RoleID,
    bool IsActive
) : IRequest<UserRoleDto?>;

}
