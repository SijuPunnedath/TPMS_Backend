using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Auth.Commands;
using TPMS.Application.Features.Auth.DTOs;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) { _mediator = mediator; }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var id = await _mediator.Send(new RegisterUserCommand(dto));
            return CreatedAtAction(nameof(Register), new { id }, new { UserID = id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var result = await _mediator.Send(new LoginUserCommand(dto, ip));
            // returning tokens in body; consider setting refresh token as HttpOnly cookie in production
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var result = await _mediator.Send(new RefreshTokenCommand(refreshToken, ip));
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var result = await _mediator.Send(new RevokeTokenCommand
            {
                Token = refreshToken,
                IpAddress = ip
            });

            if (!result)
                return BadRequest(new { Message = "Invalid or already revoked token." });

            return Ok(new { Message = "Logged out successfully." });
        }

        //[HttpPost("revoke")]
        //public async Task<IActionResult> Revoke([FromBody] string token)
        //{
        //    var ok = await _mediator.Send(new RevokeTokenCommand(token));
        //    if (!ok) return NotFound();
        //    return Ok();
        //}

    }
}
