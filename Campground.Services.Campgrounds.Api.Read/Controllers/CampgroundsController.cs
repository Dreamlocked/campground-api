using Campground.Services.Campgrounds.Api.Read.Querys.Campgrounds.GetById;
using Campground.Services.Campgrounds.Api.Read.Querys.Users.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Campground.Services.Campgrounds.Api.Read.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampgroundsController(ISender mediator) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var campground = await _mediator.Send(new GetCampgroundByIdQuery(id));
            return Ok(campground);
        }
    }
}
