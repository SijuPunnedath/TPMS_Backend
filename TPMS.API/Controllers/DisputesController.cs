using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Disputes.Commands;

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
            => Ok(await _mediator.Send(command));

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(
            int id,
            AddDisputeCommentCommand command)
            => Ok(await _mediator.Send(command with { DisputeId = id }));
    }
}
