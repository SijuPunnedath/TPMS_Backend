using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.LeaseAlert.Commands;
using TPMS.Application.Features.LeaseAlert.DTOs;
using TPMS.Application.Features.LeaseAlert.Queries;

namespace TPMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaseAlertsController : ControllerBase
{
    private readonly IMediator _mediator;
    public LeaseAlertsController(IMediator mediator) => _mediator = mediator;
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var alerts = await _mediator.Send(new GetAllLeaseAlertsQuery());
        return Ok(alerts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var alert = await _mediator.Send(new GetLeaseAlertByIdQuery(id));
        if (alert == null) return NotFound();
        return Ok(alert);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LeaseAlertDtoCrud dto)
    {
        var id = await _mediator.Send(new CreateLeaseAlertCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id }, new { AlertID = id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] LeaseAlertDtoCrud dto)
    {
        dto.AlertID = id;
        var result = await _mediator.Send(new UpdateLeaseAlertCommand(dto));
        if (!result) return NotFound();
        return Ok(new { Message = "Lease alert updated successfully" });
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteLeaseAlertCommand(id));
        if (!result) return NotFound();
        return Ok(new { Message = "Lease alert deleted" });
    }

    [HttpPatch("{id:int}/softdelete")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var result = await _mediator.Send(new SoftDeleteLeaseAlertCommand(id));
        return result ? Ok(new { Message = "Lease alert soft deleted" }) : NotFound();
    }
    
    [HttpPatch("{id:int}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var result = await _mediator.Send(new RestoreLeaseAlertCommand(id));
        return result ? Ok(new { Message = "Lease alert restored" }) : NotFound();
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingAlerts()
    {
        var result = await _mediator.Send(new GetPendingLeaseAlertsQuery());
        return Ok(result);
    }
}