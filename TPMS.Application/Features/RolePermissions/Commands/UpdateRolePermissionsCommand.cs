using MediatR;
using TPMS.Application.Features.RolePermissions.DTOs;

namespace TPMS.Application.Features.RolePermissions.Commands;

public record UpdateRolePermissionsCommand(UpdateRolePermissionsDto Dto)
    : IRequest<bool>;
