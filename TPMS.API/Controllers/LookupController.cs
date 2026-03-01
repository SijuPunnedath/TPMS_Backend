using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Lookups.Queries;

namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LookupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("available-outbound")]
        public async Task<IActionResult> GetAvailableOutbound(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetAvailableOutboundPropertiesQuery(),
                cancellationToken);

            return Ok(result);
        }
        [HttpGet("available-inbound")]
        public async Task<IActionResult> GetAvailableInbound(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetAvailableInboundPropertiesQuery(),
                cancellationToken);

            return Ok(result);
        }
        
        [HttpGet("tenants")]
        public async Task<IActionResult> GetTenants() =>
            Ok(await _mediator.Send(new GetTenantLookupQuery()));

        [HttpGet("landlords")]
        public async Task<IActionResult> GetLandlords() =>
            Ok(await _mediator.Send(new GetLandlordLookupQuery()));

        [HttpGet("properties")]
        public async Task<IActionResult> GetProperties() =>
            Ok(await _mediator.Send(new GetPropertyLookupQuery()));
        
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles() =>
            Ok(await _mediator.Send(new GetRoleLookupQuery()));

        [HttpGet("penalty-policies")]
        public async Task<IActionResult> GetPenaltyPolicies() =>
            Ok(await _mediator.Send(new GetPenaltyPolicyLookupQuery()));

        [HttpGet("owner-types")]
        public async Task<IActionResult> GetOwnerTypes() =>
            Ok(await _mediator.Send(new GetOwnerTypeLookupQuery()));

        [HttpGet("document-types")]
        public async Task<IActionResult> GetDocumentTypes() =>
            Ok(await _mediator.Send(new GetDocumentTypeLookupQuery()));
        
        
        [HttpGet("doc-categories")]
        public async Task<IActionResult> GetDocumentCategories()
        {
            var result = await _mediator.Send(new GetDocumentCategoryLookupQuery());
            return Ok(result);
        }
    }
}
