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

namespace campground_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(ReviewService reviewService) : ControllerBase
    {
        private readonly ReviewService _reviewService = reviewService;

        [HttpGet("/api/campground/{id}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewListDto>>> GetReviews(int id)
        {
            return await _reviewService.GetByCampgroundId(id);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewCreateDto>> PostReview(ReviewCreateDto reviewDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var newReview = await _reviewService.Create(int.Parse(userId), reviewDto);
            return Ok(newReview);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, ReviewCreateDto reviewDto)
        {
            var updatedReview = await _reviewService.Update(id, reviewDto);
            if(updatedReview == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Review>> DeleteUser(int id)
        {
            var review = await _reviewService.Delete(id);
            if (review == null)
            {
                return NotFound();
            }

            return review;
        }
    }
}
