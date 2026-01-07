using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Leases.Commands;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Application.Features.Leases.Queries;
using TPMS.Application.Features.RenewLease.Commands;
using TPMS.Application.Features.RenewLease.DTOs;
using TPMS.Application.Features.RenewLease.Queries;
using TPMS.Domain.Enums;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeasesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LeasesController> _logger;
        public LeasesController(IMediator mediator,ILogger<LeasesController> logger)
        {
                _mediator = mediator;
                _logger = logger;
        }

        // ✅ Create a new lease (with rent schedule generation)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LeaseCreateDto leaseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var leaseId = await _mediator.Send(new CreateLeaseCommand( leaseDto));

            _logger.LogInformation($"Lease created successfully: ID={leaseId}");
            return CreatedAtAction(nameof(GetLeasesById), new { id = leaseId }, new { LeaseID = leaseId });
        }
        
        [HttpGet]
        // GET api/Leases
        public async Task<IActionResult> GetAll([FromQuery] GetAllLeasesQuery query)
        {
            var leases = await _mediator.Send(query);
            return Ok(leases);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeasesById(int id)
        {
            var result = await _mediator.Send(new GetLeaseByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        
        [HttpGet("{Id}/alerts")]
        public async Task<IActionResult> GetAlerts(int Id)
        {
            var result = await _mediator.Send(new GetLeaseWithScheduleAlertsByIdQuery(Id));

            return Ok(result);
        }

        // GET: api/Leases/search?search=...
        [HttpGet("search")]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var leases = await _mediator.Send(new GetAllLeasesBySearchTermQuery { SearchTerm = search });
            return Ok(leases);
        }
        
        [HttpGet("search-by-type")]
        public async Task<IActionResult> GetLeasesByType([FromQuery] LeaseType leaseType, [FromQuery] string? term)
        {
            var query = new GetLeasesByTypeQuery
            {
                LeaseType = leaseType,
                SearchTerm = term
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpGet("search-paged")]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] LeaseType leaseType,
            [FromQuery] string? term,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetLeasesByTypePagedQuery
            {
                LeaseType = leaseType,
                SearchTerm = term,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpPut("{leaseId:int}")]
        public async Task<IActionResult> UpdateLease(
            int leaseId,
            [FromBody] LeaseUpdateDto leaseDto,
            CancellationToken cancellationToken)
        {
            if (leaseDto == null)
                return BadRequest("Lease data is required.");

            // Ensure route ID is used (important!)
            leaseDto.LeaseID = leaseId;

            var command = new UpdateLeaseCommand(leaseDto);

            var updated = await _mediator.Send(command, cancellationToken);

            if (!updated)
                return NotFound($"Lease with ID {leaseId} not found.");

            return NoContent(); // 204
        }
        
        //Lease renewal
        
        [HttpPost("{leaseId}/renew")]
        public async Task<IActionResult> RenewLease(
            int leaseId,
            [FromBody] RenewLeaseCommand command)
        {
            if (leaseId != command.LeaseID)
                return BadRequest("Lease ID mismatch.");

            var renewalId = await _mediator.Send(command);
            return Ok(new { LeaseRenewalID = renewalId });
        }


        //Terminate Lease.
        [HttpPost("{leaseId}/terminate")]
        public async Task<IActionResult> TerminateLease(
            int leaseId,
            [FromBody] TerminateLeaseCommand command)
        {
            if (leaseId != command.LeaseID)
                return BadRequest("Lease ID mismatch.");

            var terminationId = await _mediator.Send(command);

            return Ok(new { LeaseTerminationID = terminationId });
        }
        
        //Termination settlement
        /// <summary>
        /// Settle or Dispute a Lease Termination
        /// </summary>
        /// <param name="leaseTerminationId">LeaseTerminationID</param>
        /// <param name="request">Settlement data</param>
        [HttpPut("terminations/{leaseTerminationId}/settlement")]
        public async Task<IActionResult> UpdateSettlementStatus(
            int leaseTerminationId,
            [FromBody] SettleLeaseTerminationDto  request)
        {
            if (string.IsNullOrWhiteSpace(request.SettlementStatus))
                return BadRequest("SettlementStatus is required.");

            var command = new SettleLeaseTerminationCommand(
                LeaseTerminationID: leaseTerminationId,
                SettlementStatus: request.SettlementStatus,
                ActionBy: request.ActionBy,
                Notes: request.Notes
            );

            await _mediator.Send(command);

            return Ok(new
            {
                message = "Lease termination settlement updated successfully."
            });
        }
        
        //-- Lease termination summary
        /// <summary>
        /// Get Lease Termination Summary
        /// </summary>
        /// <param name="leaseTerminationId">LeaseTerminationID</param>
        [HttpGet("terminations/{leaseTerminationId}/summary")]
        public async Task<IActionResult> GetTerminationSummary(int leaseTerminationId)
        {
            var result = await _mediator.Send(
                new GetLeaseTerminationSummaryQuery(leaseTerminationId));

            return Ok(result);
        }

    }
}
