using Campground.Services.Campgrounds.Api.Write.Commands.Users.Create;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Campground.Services.Campgrounds.Consumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(ISender mediator) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var createResult = await _mediator.Send(command);
            return Ok(createResult);
        }
    }
}
