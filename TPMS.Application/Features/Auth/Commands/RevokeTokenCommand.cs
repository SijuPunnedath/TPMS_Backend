using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Auth.Commands
{
    public record RevokeTokenCommand : IRequest<bool>
    {
        public string Token { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
    }
}
