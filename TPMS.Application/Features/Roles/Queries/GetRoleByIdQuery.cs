using MediatR;
using TPMS.Application.Features.Roles.DTOs;

namespace TPMS.Application.Features.Roles.Queries;

public record GetRoleByIdQuery(int RoleID) : IRequest<RoleDto?>;