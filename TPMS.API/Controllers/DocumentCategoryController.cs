using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.DocumentCategories.Queries;
using TPMS.Application.Features.DocumentCategory.Commands;
using TPMS.Application.Features.DocumentCategory.DTOs;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Create(DocumentCategoryDto dto)
        {
            var result = await _mediator.Send(new CreateDocumentCategoryCommand(dto));
            return Ok(result);
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, DocumentCategoryDto dto)
        {
            dto.DocumentCategoryID = id;  // ensure correct ID

            var success = await _mediator.Send(new UpdateDocumentCategoryCommand(dto));
            if (!success)
                return NotFound("Document category not found.");

            return Ok("Updated successfully.");
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var success = await _mediator.Send(new SoftDeleteDocumentCategoryCommand(id));

            if (!success)
                return NotFound("Document category not found.");

            return Ok("Deleted successfully.");
        }
        
        [HttpPut("restore/{id:int}")]
        public async Task<IActionResult> Restore(int id)
        {
            var success = await _mediator.Send(new RestoreDocumentCategoryCommand(id));

            if (!success)
                return NotFound("Document category not found.");

            return Ok("Restored successfully.");
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
        {
            var categories = await _mediator.Send(
                new GetAllDocumentCategoriesQuery { IncludeInactive = includeInactive });

            return Ok(categories);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _mediator.Send(new GetDocumentCategoryByIdQuery(id));

            if (category == null)
                return NotFound("Document category not found.");

            return Ok(category);
        }
        
        [HttpGet("tree")]
        public async Task<IActionResult> GetCategoryTree()
        {
            var result = await _mediator.Send(new GetDocumentCategoryTreeQuery());
            return Ok(result);
        }
        
     /*   [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(
                new GetDocumentCategoryPagedQuery(pageNumber, pageSize));

            return Ok(result);
        } */
        
    }
}
