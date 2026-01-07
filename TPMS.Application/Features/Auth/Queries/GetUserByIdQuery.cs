using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Auth.DTOs;

namespace TPMS.Application.Features.Auth.Queries
{
    public record GetUserByIdQuery(int UserId) : IRequest<UserDto?>;
}
