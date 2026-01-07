using Amazon.Runtime.Internal;
using DocumentFormat.OpenXml.Drawing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.RolePermissions.DTOs;

namespace TPMS.Application.Features.RolePermissions.Commands
{
    public class CreateRolePermissionCommand :IRequest<RolePermissionDto>
    {
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public bool isAllowed { get; set; } 
    }
}
