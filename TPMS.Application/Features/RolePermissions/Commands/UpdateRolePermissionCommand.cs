using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.RolePermissions.DTOs;

namespace TPMS.Application.Features.RolePermissions.Commands
{
    public class UpdateRolePermissionCommand :IRequest<RolePermissionDto?>
    {
        public int RolePermissionID { get; set; }
        public bool IsAllowed { get; set; }
    }
}
