using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.OwnerTypes.Commands;
using TPMS.Application.Features.OwnerTypes.DTOs;
using TPMS.Application.Features.OwnerTypes.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerTypesController : ControllerBase
    {
       
        private readonly IMediator _mediator;
        public OwnerTypesController(IMediator mediator) => _mediator = mediator;

            [HttpGet]
            public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
            {
                var result = await _mediator.Send(new GetAllOwnerTypesQuery(includeDeleted));
                return Ok(result);
            }

            [HttpGet("{id:int}")]
            public async Task<IActionResult> GetById(int id)
            {
                var result = await _mediator.Send(new GetOwnerTypeByIdQuery(id));
                return result == null ? NotFound() : Ok(result);
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] OwnerTypeDto dto)
            {
                var id = await _mediator.Send(new CreateOwnerTypeCommand(dto, CreatedBy: 1));
                return CreatedAtAction(nameof(GetAll), new { id }, new { OwnerTypeID = id });
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] OwnerTypeDto dto)
            {
                dto.OwnerTypeID = id;
                var success = await _mediator.Send(new UpdateOwnerTypeCommand(dto, UpdatedBy: 1));
                return success ? Ok() : NotFound();
            }

            [HttpDelete("{id:int}")]
            public async Task<IActionResult> Delete(int id)
            {
                var result = await _mediator.Send(new DeleteOwnerTypeCommand(id));
                return result ? Ok(new { Message = "Owner type deleted successfully" }) : NotFound();
            }
            [HttpDelete("soft/{id}")]
            public async Task<IActionResult> SoftDelete(int id)
            {
                var success = await _mediator.Send(new SoftDeleteOwnerTypeCommand(id, UpdatedBy: 1));
                return success ? Ok() : NotFound();
            }
            
            [HttpPut("restore/{id}")]
            public async Task<IActionResult> Restore(int id)
            {
                var success = await _mediator.Send(new RestoreOwnerTypeCommand(id, UpdatedBy: 1));
                return success ? Ok() : NotFound();
            }
            
            [HttpGet("tree")]
            public async Task<IActionResult> GetOwnerTree(
                CancellationToken cancellationToken)
            {
                var result = await _mediator.Send(
                    new GetOwnerTreeQuery(),
                    cancellationToken);

                return Ok(result);
            }
    }
}
