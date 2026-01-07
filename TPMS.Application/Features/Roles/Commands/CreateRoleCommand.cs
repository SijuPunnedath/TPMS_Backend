using MediatR;
using TPMS.Application.Features.Roles.DTOs;

namespace TPMS.Application.Features.Roles.Commands;

public record CreateRoleCommand(CreateRoleDto Role) : IRequest<int>;