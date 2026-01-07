using MediatR;
using TPMS.Application.Features.Permissions.DTOs;

namespace TPMS.Application.Features.Permissions.Queries;

public record GetPermissionByIdQuery(int PermissionID) : IRequest<PermissionDto?>;