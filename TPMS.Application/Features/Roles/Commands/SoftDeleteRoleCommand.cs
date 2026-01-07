using MediatR;

namespace TPMS.Application.Features.Roles.Commands;

public record SoftDeleteRoleCommand(int RoleID) : IRequest<bool>;