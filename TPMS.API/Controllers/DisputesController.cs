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
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetDisputeByIdQuery(id));
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateDisputeCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

      
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
        {
            var result = await _mediator.Send(new DeleteDisputeCommand(id, userId));
            return Ok(result);
        } 
      

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> Assign(int id, [FromBody] int userId)
        {
            await _mediator.Send(new AssignDisputeCommand(id, userId));
            return NoContent();
        }
        
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            // Get TenantId from claims (recommended)   
           // var tenantId = int.Parse(User.FindFirst("TenantId")!.Value);

            var result = await _mediator.Send(
                new GetDisputeDashboardQuery());

            return Ok(result);
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
