using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.Assets.Commands;
using TPMS.Application.Features.Assets.DTOs;
using TPMS.Application.Features.Assets.Queries;

namespace TPMS.API.Controllers
{
    [ApiController]
    [Route("api/assets")]
    public class AssetsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAssetDto dto)
            => Ok(await _mediator.Send(new CreateAssetCommand(dto)));

        [HttpGet("by-property/{propertyId}")]
        public async Task<IActionResult> GetByProperty(int propertyId)
            => Ok(await _mediator.Send(new GetAssetsByPropertyQuery(propertyId)));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAssetCommand command)
            => Ok(await _mediator.Send(command with { AssetId = id }));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _mediator.Send(new DeleteAssetCommand(id)));
    }

}
