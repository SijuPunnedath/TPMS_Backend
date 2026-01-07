using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Roles.DTOs;

namespace TPMS.Application.Features.Roles.Queries;

public record GetAllRolesWithPermissionsQuery()
    : IRequest<List<RoleWithPermissionsDto>>;
