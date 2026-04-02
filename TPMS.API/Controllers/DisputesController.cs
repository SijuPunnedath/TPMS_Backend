using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Disputes.Commands;
using TPMS.Application.Features.Disputes.Queries;
using TPMS.Domain.Enums;


namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisputesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DisputesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateDisputeCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetDisputeByIdQuery(id));

            return result.IRet == 1 ? Ok(result) : NotFound(result);
        }
     /*   [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetDisputeByIdQuery(id));
            return Ok(result);
        } */

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateDisputeCommand command)
        {
            if (id != command.Id)
                return BadRequest("Invalid request");

            var result = await _mediator.Send(command);

            return Ok(result); 
        }
        
      /*  [HttpPut]
        public async Task<IActionResult> Update(UpdateDisputeCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }*/

      
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
        {
            var result = await _mediator.Send(new DeleteDisputeCommand(id, userId));
            return Ok(result);
        } 
      
        [HttpPut("{id}/assign")]
        public async Task<IActionResult> Assign(int id, AssignDisputeCommand command)
        {
            if (id != command.Id)
                return BadRequest("Invalid request");

            var result = await _mediator.Send(command);

            return Ok(result);
        }
        
    
        
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _mediator.Send(new GetDisputeDashboardQuery());

            return result.IRet == 1 ? Ok(result) : BadRequest(result);
        }
        
        
     [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] DisputeStatus? status = null,
            [FromQuery] DisputePriority? priority = null)
        {
          //  var tenantId = int.Parse(User.FindFirst("TenantId")!.Value);

            var result = await _mediator.Send(
                new GetAllDisputesQuery(
                    pageNumber,
                    pageSize,
                    status,
                    priority));

            return Ok(result);
        } 
    }
}
