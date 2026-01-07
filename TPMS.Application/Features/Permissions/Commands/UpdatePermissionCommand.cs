using MediatR;
using TPMS.Application.Features.Permissions.DTOs;

namespace TPMS.Application.Features.Permissions.Commands;

public record UpdatePermissionCommand(
    int PermissionId,
    UpdatePermissionDto Dto) : IRequest<bool>;