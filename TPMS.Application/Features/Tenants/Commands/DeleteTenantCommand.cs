using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Tenants.Commands
{
    public class DeleteTenantCommand :IRequest<bool>
    {
        public int TenantId { get; }
        public DeleteTenantCommand(int tenantId) => TenantId = tenantId;
    }

}
