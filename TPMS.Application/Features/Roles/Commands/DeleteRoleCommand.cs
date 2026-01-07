using MediatR;

namespace TPMS.Application.Features.Roles.Commands;

public record DeleteRoleCommand(int RoleID) : IRequest<bool>;
