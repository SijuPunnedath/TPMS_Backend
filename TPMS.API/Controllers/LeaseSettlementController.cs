using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Leases.Commands;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaseSettlementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaseSettlementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSettlement(
            CreateLeaseSettlementCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteSettlement(int id)
        {
            await _mediator.Send(new CompleteLeaseSettlementCommand
            {
                LeaseSettlementId = id
            });

            return NoContent();
        }
    }
}
