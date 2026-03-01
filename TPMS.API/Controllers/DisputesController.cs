using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace TPMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisputesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DisputesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
       
    }
}
