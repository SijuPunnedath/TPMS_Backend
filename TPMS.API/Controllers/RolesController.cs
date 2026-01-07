using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Roles.Commands;
using TPMS.Application.Features.Roles.DTOs;
using TPMS.Application.Features.Roles.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            var id = await _mediator.Send(new CreateRoleCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id }, new { RoleID = id });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = true)
        {
            var roles = await _mediator.Send(new GetAllRolesQuery(includeInactive));
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _mediator.Send(new GetRoleByIdQuery(id));
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto dto)
        {
            var result = await _mediator.Send(new UpdateRoleCommand(id, dto));
            return result ? Ok() : NotFound();
        }
        
        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _mediator.Send(new SoftDeleteRoleCommand(id));
            return NoContent();
        }

        [HttpPatch("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            await _mediator.Send(new RestoreRoleCommand(id));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            await _mediator.Send(new DeleteRoleCommand(id));
            return NoContent();
        }
        
        [HttpGet("roles/permissions")]
        public async Task<IActionResult> GetAllRolesWithPermissions()
        {
            var result = await _mediator.Send(new GetAllRolesWithPermissionsQuery());
            return Ok(result);
        }
    }
}
