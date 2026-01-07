using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Penaltyploicy.Commands;
using TPMS.Application.Features.Penaltyploicy.DTOs;
using TPMS.Application.Features.Penaltyploicy.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenaltyPoliciesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PenaltyPoliciesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatepenaltyDto dto)
        {
            var id = await _mediator.Send(new CreatePenaltyPolicyCommand { Policy = dto });
            return CreatedAtAction(nameof(GetById), new { id }, new { PolicyID = id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var policy = await _mediator.Send(new GetPenaltyPolicyByIdQuery { PolicyId = id });
            return policy == null ? NotFound() : Ok(policy);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _mediator.Send(new GetAllPenaltyPoliciesQuery());
            return Ok(list);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PenaltyPolicyDto dto)
        {
            dto.PenaltyPolicyID = id;
            var result = await _mediator.Send(new UpdatePenaltyPolicyCommand { Policy = dto });
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePenaltyPolicyCommand { PolicyId = id });
            return result ? Ok() : NotFound();
        }
    }
}
