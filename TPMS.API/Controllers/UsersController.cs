using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Auth.Queries;
using TPMS.Application.Features.Users.Commands;
using TPMS.Application.Features.Users.DTOs;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator) { _mediator = mediator; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var id = await _mediator.Send(new CreateUserCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id }, new { UserID = id });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _mediator.Send(new GetAllUsersQuery());
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            var updated = await _mediator.Send(new UpdateUserCommand { UserId = id, User = userDto });
            if (updated == null)
                return NotFound(new { Message = $"User with ID {id} not found." });
            return Ok(updated);
        }

        [HttpPatch("{id:int}/soft-delete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            bool result = await _mediator.Send(new SoftDeleteUserCommand(id));
            if (!result)
                return NotFound(new { Message = $"User with ID {id} not found." });
            return Ok(new { Message = "User deactivated successfully." });
        }

    }
}
