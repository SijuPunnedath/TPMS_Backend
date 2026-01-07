using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Tenants.DTOs;

namespace TPMS.Application.Features.Tenants.Commands;

public record UpdateTenantCommand(int TenantID, CreateTenantDto Tenant) : IRequest<bool>;


/*{
    public  class UpdateTenantCommand :  IRequest<bool>
    {
        public int TenantId { get; }
        public TenantDto Tenant { get; }

        public UpdateTenantCommand(int tenantId, TenantDto tenant)
        {
            TenantId = tenantId;
            Tenant = tenant;
        }

    }
}*/
