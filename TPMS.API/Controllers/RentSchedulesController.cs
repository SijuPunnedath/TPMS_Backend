using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.RentSchedules.Commands;
using TPMS.Application.Features.RentSchedules.DTOs;
using TPMS.Application.Features.RentSchedules.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentSchedulesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RentSchedulesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllRentSchedulesQuery());
            return Ok(result);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetRentScheduleByIdQuery(id));
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("lease/{leaseId:int}")]
        public async Task<IActionResult> GetByLeaseId(int leaseId)
        {
            var result = await _mediator.Send(new GetRentSchedulesByLeaseIdQuery(leaseId));
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RentScheduleDtoCrud dto)
        {
            var id = await _mediator.Send(new CreateRentScheduleCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id }, new { ScheduleID = id });
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RentScheduleDtoCrud dto)
        {
            dto.ScheduleID = id;
            var result = await _mediator.Send(new UpdateRentScheduleCommand(dto));
            if (!result) return NotFound();
            return Ok(new { Message = "Rent schedule updated successfully" });
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteRentScheduleCommand(id));
            return result ? Ok(new { Message = "Rent schedule deleted successfully" }) : NotFound();
        }
    }
}
