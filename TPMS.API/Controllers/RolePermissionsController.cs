using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.RolePermissions.Commands;
using TPMS.Application.Features.RolePermissions.DTOs;
using TPMS.Application.Features.RolePermissions.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RolePermissionsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("role/{roleId:int}")]
        public async Task<IActionResult> GetByRole(int roleId)
        {
            var result = await _mediator.Send(new GetRolePermissionsByRoleIdQuery(roleId));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRolePermissionCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByRole), new { roleId = result.RoleID }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRolePermissionCommand command)
        {
            command.RolePermissionID = id;
            var updated = await _mediator.Send(command);
            if (updated == null)
                return NotFound(new { Message = $"RolePermission with ID {id} not found." });
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _mediator.Send(new DeleteRolePermissionCommand(id));
            if (!deleted)
                return NotFound(new { Message = $"RolePermission with ID {id} not found." });
            return Ok(new { Message = "Permission removed successfully." });
        }
        
        /// <summary>
        /// Assign multiple permissions to a role
        /// </summary>
        [HttpPost("{roleId}/permissions")]
        public async Task<IActionResult> AssignPermissions(
            int roleId,
            [FromBody] List<int> permissionIds)
        {
            await _mediator.Send(new AssignRolePermissionsCommand(
                new AssignRolePermissionsDto
                {
                    RoleID = roleId,
                    PermissionIDs = permissionIds
                }));

            return Ok(new { message = "Permissions assigned successfully." });
        }
        
        /// <summary>
        /// Remove multiple permissions from a role
        /// </summary>
        [HttpDelete("{roleId}/permissions")]
        public async Task<IActionResult> RemovePermissions(
            int roleId,
            [FromBody] List<int> permissionIds)
        {
            await _mediator.Send(new RemoveRolePermissionsCommand(
                new RemoveRolePermissionsDto
                {
                    RoleID = roleId,
                    PermissionIDs = permissionIds
                }));

            return Ok(new { message = "Permissions removed successfully." });
        }
        
        
        [HttpPut("{roleId}/permissions")]
        public async Task<IActionResult> UpdateRolePermissions(
            int roleId,
            [FromBody] List<int> allowedPermissionIds)
        {
            await _mediator.Send(new UpdateRolePermissionsCommand(
                new UpdateRolePermissionsDto
                {
                    RoleID = roleId,
                    AllowedPermissionIDs = allowedPermissionIds
                }));

            return Ok(new { message = "Role permissions updated successfully." });
        }

        
        
        
        
    }
}
