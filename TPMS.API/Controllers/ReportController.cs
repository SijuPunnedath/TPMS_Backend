using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Reports.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
                _mediator = mediator;
        }
        
        [HttpGet("documents-report")]
        public async Task<IActionResult> GetDocumentsReport(
            [FromQuery] GetDocumentReportQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("landlord-report")]
        public async Task<IActionResult> GetlanlordReport([FromQuery] GetLandlordReportQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpGet("overduepayment-report")]
        public async Task<IActionResult> GetOverduePaymentsReport([FromQuery] GetOverduePaymentsReportQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpGet("Rentdue-report")]
        public async Task<IActionResult> GetRentdueReport([FromQuery]  GetRentDueReportQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpGet("Rentpaymenthistory-report")]
        public async Task<IActionResult> GetrentpaymenthistoryReport([FromQuery]  GetRentPaymentHistoryQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("Tenant-report")]
        public async Task<IActionResult> GetTenantReport([FromQuery]  GetTenantReportQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpGet("lease-expiry")]
        public async Task<IActionResult> GetLeaseExpiryReport(
            [FromQuery] GetLeaseExpiryReportQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
