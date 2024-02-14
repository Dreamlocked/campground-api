using Campground.Services.Campgrounds.Api.Read.Querys.Users.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Campground.Services.Campgrounds.Api.Read.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(ISender mediator) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usersResult = await _mediator.Send(new GetAllUsersQuery());
            return Ok(usersResult);
        }

    }
}
