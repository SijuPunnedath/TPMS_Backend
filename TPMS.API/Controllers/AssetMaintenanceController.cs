using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Maintenance.Commands;
using TPMS.Application.Features.Maintenance.DTOs;
using TPMS.Application.Features.Maintenance.Queries;

namespace TPMS.API.Controllers
{
    [ApiController]
    [Route("api/asset-maintenance")]
    public class AssetMaintenanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssetMaintenanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ApiResponse<int>> Create(CreateAssetMaintenanceDto dto)
            => await _mediator.Send(new CreateAssetMaintenanceCommand(dto));

        [HttpPut]
        public async Task<ApiResponse<bool>> Update(UpdateAssetMaintenanceDto dto)
            => await _mediator.Send(new UpdateAssetMaintenanceCommand(dto));

        [HttpDelete("{id}")]
        public async Task<ApiResponse<bool>> Delete(int id)
            => await _mediator.Send(new DeleteAssetMaintenanceCommand(id));

        [HttpGet("{id}")]
        public async Task<ApiResponse<AssetMaintenanceDto>> GetById(int id)
            => await _mediator.Send(new GetAssetMaintenanceByIdQuery(id));

        [HttpGet("asset/{assetId}")]
        public async Task<ApiResponse<List<AssetMaintenanceDto>>> GetByAsset(int assetId)
            => await _mediator.Send(new GetAssetMaintenancesByAssetQuery(assetId));
    }

}
