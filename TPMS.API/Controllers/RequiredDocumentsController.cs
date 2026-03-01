using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.RequiredDocuments.Commands;
using TPMS.Application.Features.RequiredDocuments.Queries;

namespace TPMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequiredDocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RequiredDocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(CreateRequiredDocumentCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive)
        {
            var result = await _mediator.Send(new GetRequiredDocumentsQuery 
            { 
                IsActive = isActive 
            });

            return Ok(result);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetRequiredDocumentByIdQuery(id));
            return Ok(result);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRequiredDocumentCommand command)
        {
            if (id != command.RequiredDocumentID)
                return BadRequest();

            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE (Soft)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteRequiredDocumentCommand(id));
            return NoContent();
        }
    }

}
