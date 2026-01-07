using MediatR;
using TPMS.Application.Features.Users.DTOs;

namespace TPMS.Application.Features.Users.Commands;

public record CreateUserCommand(CreateUserDto User) : IRequest<int>;