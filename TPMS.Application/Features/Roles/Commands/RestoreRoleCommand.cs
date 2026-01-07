using MediatR;

namespace TPMS.Application.Features.Roles.Commands;

public record RestoreRoleCommand(int RoleID) : IRequest<bool>;