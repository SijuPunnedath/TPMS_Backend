using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.RolePermissions.DTOs;

namespace TPMS.Application.Features.RolePermissions.Queries
{ 

    public record GetRolePermissionsByRoleIdQuery(int RoleId) : IRequest<List<RolePermissionDto>>;
    
}
