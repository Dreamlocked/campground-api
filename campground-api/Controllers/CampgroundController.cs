using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using campground_api.Models;
using campground_api.Services;
using campground_api.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using campground_api.Utils;
using Microsoft.AspNetCore.SignalR;

namespace campground_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CampgroundController(CampgroundService campgroundService) : ControllerBase
    {
        private readonly CampgroundService _campgroundService = campgroundService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CampgroundListDto>>> GetCampgrounds()
        {
            return await _campgroundService.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CampgroundGetDto>> GetCampground(int id)
        {
            var campground = await _campgroundService.Get(id);

            if (campground == null)
            {
                return NotFound();
            }

            return campground;
        }

        [HttpPost]
        public async Task<ActionResult<CampgroundGetDto>> PostCampground([FromForm] CampgroundCreateDto campground)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value; 
            var newCampground = await _campgroundService.Create(int.Parse(userId!), campground);
            return Ok(newCampground);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampground(int id, [FromForm] CampgroundUpdateDto campground)
        {
            var updatedCampground = await _campgroundService.Update(id, campground);

            if(updatedCampground == null) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Campground>> DeleteCampground(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")!.Value);
            try
            {
                var campground = await _campgroundService.Delete(userId, id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
