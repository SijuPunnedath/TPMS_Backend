using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Permissions.DTOs;

namespace TPMS.Application.Features.Permissions.Queries;

public record GetPermissionsByModuleQuery(string Module)
    : IRequest<List<PermissionDto>>;