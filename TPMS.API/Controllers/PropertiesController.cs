using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Properties.Commands;
using TPMS.Application.Features.Properties.DTOs;
using TPMS.Application.Features.Properties.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertiesController(IMediator mediator) => _mediator = mediator;
       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePropertyDto dto)
        {
            var result = await _mediator.Send(new CreatePropertyCommand(dto));
            return Ok(result);
        }
       
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var property = await _mediator.Send(new GetPropertyByIdQuery(id));
            if (property == null) return NotFound();
            return Ok(property);
        }
      
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var properties = await _mediator.Send(new GetAllPropertiesQuery());
            return Ok(properties);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PropertyDto propertyDto)
        {
            var success = await _mediator.Send(new UpdatePropertyCommand(id, propertyDto));
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeletePropertyCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id:int}/softdelete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var success = await _mediator.Send(new SoftDeletePropertyCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id:int}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var success = await _mediator.Send(new RestorePropertyCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeleted()
        {
            var properties = await _mediator.Send(new GetDeletedPropertiesQuery());
            return Ok(properties);
        }

        [HttpGet("GetAlExtended")]
        public async Task<IActionResult> GetAlExtended(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? type = null,
        [FromQuery] string? city = null,
        [FromQuery] int? landlordId = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = false)
        {
            var query = new GetAllPropertiesExtendedQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Type = type,
                City = city,
                LandlordId = landlordId,
                Search = search,
                SortBy = sortBy,
                SortDesc = sortDesc
            };

            var properties = await _mediator.Send(query);
            return Ok(properties);
        }
        
        [HttpGet("{id}/documents")]
        public async Task<IActionResult> GetDocuments(int id)
        {
            var result = await _mediator.Send(
                new GetPropertyDocumentsQuery(id));

            return Ok(result);
        }

        [HttpGet("{id}/documents/compliance")]
        public async Task<IActionResult> GetCompliance(int id)
        {
            var result = await _mediator.Send(
                new GetPropertyComplianceQuery(id));

            return Ok(result);
        }

    }
}
