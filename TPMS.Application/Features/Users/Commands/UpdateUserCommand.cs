using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Auth.DTOs;
using TPMS.Application.Features.Users.DTOs;

namespace TPMS.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserDto?>
    {
        public int UserId { get; set; }
        public UpdateUserDto User { get; set; } = new UpdateUserDto();
    }
}
