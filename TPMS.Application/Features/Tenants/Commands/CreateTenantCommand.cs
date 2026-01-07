using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Tenants.DTOs;

namespace TPMS.Application.Features.Tenants.Commands;

public record CreateTenantCommand(CreateTenantDto Tenant) : IRequest<int>;



