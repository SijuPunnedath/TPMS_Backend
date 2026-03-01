using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Tenants.Commands;
using TPMS.Application.Features.Tenants.DTOs;
using TPMS.Application.Features.Tenants.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTenantDto tenantDto)
        {
            var id = await _mediator.Send(new CreateTenantCommand(tenantDto));
            return CreatedAtAction(nameof(GetById), new { id }, new { TenantID = id });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, [FromQuery] string? search = null)
        {
            var tenants = await _mediator.Send(new GetAllTenantsQuery(includeDeleted, search));
            return Ok(tenants);
        }
        

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tenant = await _mediator.Send(new GetTenantByIdQuery(id));
            if (tenant == null) return NotFound();
            return Ok(tenant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTenantDto dto)
        {
            var result = await _mediator.Send(new UpdateTenantCommand(id, dto));
            return result ? Ok() : NotFound();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteTenantCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id:int}/softdelete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var success = await _mediator.Send(new SoftDeleteTenantCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id:int}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var success = await _mediator.Send(new RestoreTenantCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeleted()
        {
            var tenants = await _mediator.Send(new GetDeletedTenantsQuery());
            return Ok(tenants);
        }
        
        [HttpGet("lookup/tenants")]
        public async Task<ActionResult<List<TenantLookupDto>>> GetTenantLookup()
        {
            return Ok(await _mediator.Send(new GetTenantLookupQuery()));
        }

        [HttpGet("{id}/documents")]
        public async Task<IActionResult> GetDocuments(int id)
        {
            var result = await _mediator.Send(
                new GetTenantDocumentsQuery(id));

            return Ok(result);
        }

        [HttpGet("{id}/documents/compliance")]
        public async Task<IActionResult> GetCompliance(int id)
        {
            var result = await _mediator.Send(
                new GetTenantComplianceQuery(id));

            return Ok(result);
        }
    }
}
