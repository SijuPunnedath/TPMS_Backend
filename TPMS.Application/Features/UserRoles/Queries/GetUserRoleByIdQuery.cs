using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.UserRoles.DTOs;

namespace TPMS.Application.Features.UserRoles.Queries
{
    public record GetUserRoleByIdQuery(int UserRoleID) : IRequest<UserRoleDto?>;
}
