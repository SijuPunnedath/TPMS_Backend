using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPMS.Application.Features.AssetSubCategories.Commands;
using TPMS.Application.Features.AssetSubCategories.Queries;

namespace TPMS.API.Controllers
{
    [ApiController]
    [Route("api/asset-subcategories")]
    public class AssetSubCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssetSubCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAssetSubCategoryCommand command)
            => Ok(await _mediator.Send(command));

        [HttpGet("by-category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
            => Ok(await _mediator.Send(new GetAssetSubCategoriesByCategoryQuery(categoryId)));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAssetSubCategoryCommand command)
            => Ok(await _mediator.Send(command with { AssetSubCategoryId = id }));
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _mediator.Send(new DeleteAssetSubCategoryCommand(id)));
    }

}
