using MediatR;
using TPMS.Application.Features.Roles.DTOs;

namespace TPMS.Application.Features.Roles.Commands;

public record UpdateRoleCommand(int RoleID, UpdateRoleDto Role) : IRequest<bool>;

