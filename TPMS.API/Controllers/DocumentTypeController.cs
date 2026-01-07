using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.DocumentTypes.Commands;
using TPMS.Application.Features.DocumentTypes.DTOs;
using TPMS.Application.Features.DocumentTypes.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDocumentTypeCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetAll), new { id }, new { DocumentTypeID = id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllDocumentTypesQuery());
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateDocumentTypeDto dto)
        {
            await _mediator.Send(new UpdateDocumentTypeCommand(dto));
            return Ok("Updated successfully");
        }
        
        // --- Soft Delete ---
        [HttpDelete("{id}/soft-delete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _mediator.Send(new SoftDeleteDocumentTypeCommand(id));
            if (!result) return NotFound("DocumentType not found.");

            return Ok("DocumentType deleted successfully.");
        }

        // --- Restore ---
        [HttpPut("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _mediator.Send(new RestoreDocumentTypeCommand(id));
            if (!result) return NotFound("DocumentType not found.");

            return Ok("DocumentType restored successfully.");
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _mediator.Send(new GetDocumentTypeByIdQuery(id)));
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            return Ok(await _mediator.Send(new GetDocumentTypesByCategoryQuery(categoryId)));
        }
    }
}
