using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Landlords.Commands;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Application.Features.Landlords.Handlers;
using TPMS.Application.Features.Landlords.Queries;
using TPMS.Domain.Entities;


namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandlordController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LandlordController(IMediator mediator)
        {
            _mediator = mediator;

        }   

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLandlordDto landlordDto)
        {
            var id = await _mediator.Send(new CreateLandlordCommand(landlordDto));
            return CreatedAtAction(nameof(GetById), new { id }, new { LandlordID = id });
        }

        //-- GET /api/landlords/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var landlord = await _mediator.Send(new GetLandlordByIdQuery(id));
            if (landlord == null) return NotFound();
            return Ok(landlord);
        }


        //-- GET /api/landlords?page=1&pageSize=10
        [HttpGet("GetAllByPage")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllLandlordsByPageQuery(page, pageSize));
            return Ok(result);
        }

        //-- GET /api/landlords
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var landlords = await _mediator.Send(new GetAllLandlordsQuery());
            return Ok(landlords);
        }

        // GET /api/landlords?page=1&pageSize=10&search=John&sortBy=CreatedAt&sortOrder=desc
        [HttpGet("GetAllByPageSortAndFilter")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc")
        {
            // Corrected the instantiation of GetAllLandlordsBySortAndFilterQuery
            var result = await _mediator.Send(new GetAllLandlordsBySortAndFilterQuery(page, pageSize, search, sortBy, sortOrder));
            return Ok(result);
        }

        // GET /api/landlords?page=1&pageSize=10&search=John&sortBy=CreatedAt&sortOrder=desc&startDate=2025-01-01&endDate=2025-03-01
        [HttpGet("GetAllLandlordsByDate")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _mediator.Send(
                new GetAllLandlordsByDateQuery(page, pageSize, search, sortBy, sortOrder, startDate, endDate)
            );
            return Ok(result);
        }


        // GET /api/landlords/export?format=csv&search=John&sortBy=CreatedAt&sortOrder=desc
        [HttpGet("export")]
        public async Task<IActionResult> Export(
            [FromQuery] string format = "csv",
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var fileBytes = await _mediator.Send(new ExportLandlordsQuery(format, search, sortBy, sortOrder, startDate, endDate));

            var fileName = $"landlords_{DateTime.UtcNow:yyyyMMddHHmmss}.{(format == "excel" ? "xlsx" : "csv")}";
            var contentType = format == "excel"
                ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                : "text/csv";

            return File(fileBytes, contentType, fileName);
        }


        // PUT /api/landlords/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] LandlordDto landlordDto)
        {
            var success = await _mediator.Send(new UpdateLandlordCommand(id, landlordDto));
            if (!success) return NotFound();
            return NoContent(); // standard 204 on update
        }

        // DELETE /api/landlords/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteLandlordCommand(id));
            if (!success) return NotFound();
            return NoContent(); // 204 on success
        }

        // PATCH /api/landlords/{id}/restore
        [HttpPatch("{id:int}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var success = await _mediator.Send(new RestoreLandlordCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }

        // GET /api/landlords/deleted
        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeleted()
        {
            var landlords = await _mediator.Send(new GetDeletedLandlordsQuery());
            return Ok(landlords);
        }
        
       
        [HttpPatch("{id:int}/delete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var success = await _mediator.Send(new SoftDeleteLandlordCommand(id));
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
