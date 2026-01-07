using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Permissions.Commands;
using TPMS.Application.Features.Permissions.DTOs;
using TPMS.Application.Features.Permissions.Queries;

[ApiController]
[Route("api/permissions")]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllPermissionsQuery()));

    [HttpGet("module/{module}")]
    public async Task<IActionResult> GetByModule(string module)
        => Ok(await _mediator.Send(new GetPermissionsByModuleQuery(module)));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetPermissionByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePermissionDto dto)
    {
        var result = await _mediator.Send(new CreatePermissionCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.PermissionID }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePermissionDto dto)
    {
        var success = await _mediator.Send(new UpdatePermissionCommand(id, dto));
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeletePermissionCommand(id));
        return success ? NoContent() : NotFound();
    }
}