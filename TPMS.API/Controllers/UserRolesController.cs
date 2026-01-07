using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.UserRoles.Commands;
using TPMS.Application.Features.UserRoles.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserRolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUserRolesQuery());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetUserRoleByIdQuery(id));
            if (result == null)
                return NotFound(new { Message = $"UserRole with ID {id} not found." });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRoleCommand command)
        {
            var created = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = created.UserRoleID }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRoleCommand command)
        {
            command = command with { UserRoleID = id };
            var updated = await _mediator.Send(command);
            if (updated == null)
                return NotFound(new { Message = $"UserRole with ID {id} not found." });
            return Ok(updated);
        }

        [HttpPatch("{id:int}/soft-delete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            bool result = await _mediator.Send(new SoftDeleteUserRoleCommand(id));
            if (!result)
                return NotFound(new { Message = $"UserRole with ID {id} not found." });
            return Ok(new { Message = "UserRole deactivated successfully." });
        }
    }
}
