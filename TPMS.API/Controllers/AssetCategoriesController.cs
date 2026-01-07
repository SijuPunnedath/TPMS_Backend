

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.AssetCategories.Commands;
using TPMS.Application.Features.AssetCategories.Queries;

namespace TPMS.API.Controllers
{
    [ApiController]
    [Route("api/asset-categories")]
    public class AssetCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssetCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAssetCategoryCommand command)
            => Ok(await _mediator.Send(command));

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _mediator.Send(new GetAllAssetCategoriesQuery()));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAssetCategoryCommand command)
            => Ok(await _mediator.Send(command with { AssetCategoryId = id }));
    }

}