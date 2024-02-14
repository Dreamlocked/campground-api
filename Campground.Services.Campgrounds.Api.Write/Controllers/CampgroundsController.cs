using Campground.Services.Campgrounds.Api.Write.Commands.Campgrounds.Create;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Campground.Services.Campgrounds.Api.Write.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampgroundsController(ISender mediator) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCampgroundCommand command)
        {
            var createResult = await _mediator.Send(command);
            return Ok(createResult);
        }
    }
}
