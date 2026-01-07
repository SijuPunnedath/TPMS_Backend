using MediatR;
using TPMS.Application.Features.RolePermissions.DTOs;

namespace TPMS.Application.Features.RolePermissions.Commands;

public record RemoveRolePermissionsCommand(RemoveRolePermissionsDto Dto)
    : IRequest<bool>;