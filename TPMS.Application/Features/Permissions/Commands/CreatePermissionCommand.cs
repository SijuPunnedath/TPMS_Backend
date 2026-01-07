using MediatR;
using TPMS.Application.Features.Permissions.DTOs;

namespace TPMS.Application.Features.Permissions.Commands;

using MediatR;

public record CreatePermissionCommand(CreatePermissionDto Dto)
    : IRequest<PermissionDto>;
