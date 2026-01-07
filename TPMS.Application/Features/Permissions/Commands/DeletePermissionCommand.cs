using MediatR;

namespace TPMS.Application.Features.Permissions.Commands;

public record DeletePermissionCommand(int PermissionID) : IRequest<bool>;