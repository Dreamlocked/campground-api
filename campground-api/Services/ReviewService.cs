using campground_api.Models;
using campground_api.Models.Dto;
using campground_api.Utils;
using Microsoft.EntityFrameworkCore;
using System;

namespace campground_api.Services
{
    public class ReviewService(CampgroundContext context)
    {
        private readonly CampgroundContext _context = context;

        public async Task<List<ReviewListDto>> GetByCampgroundId(int id)
        {
            var reviews = await _context.Reviews
                .Include(r => r.Bookings)
                .ThenInclude(b => b.User)
                .Where(r => r.Bookings.CampgroundId == id)
                .ToListAsync();

            return reviews.Select(Mapper.MapReviewToReviewListDto).ToList();
        }

        public async Task<List<Booking>> EnableToReview(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId && b.LeavingDate < DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();
        }

        public async Task<ReviewCreateDto> Create(int userId, ReviewCreateDto reviewDto)
        {
            _context.Reviews.Add(new Review()
            {
                BookingsId = reviewDto.BookingsId,
                Body = reviewDto.Body,
                Rating = reviewDto.Rating,
                CreateAt = DateTime.Now,
            });
            await _context.SaveChangesAsync();
            return reviewDto;
        }

        public async Task<Review?> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return null;
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return review;
        }
        public async Task<Review?> Update(int id, ReviewCreateDto reviewDto)
        {

            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if(review is null) return null;

                review.Body = reviewDto.Body;
                review.Rating = reviewDto.Rating;

                await _context.SaveChangesAsync();

                return review;
            }
            catch (DbUpdateConcurrencyException) when (!ReviewExists(id))
            {
                return null;
            }
        }

        private bool ReviewExists(int id) =>
            _context.Reviews.Any(e => e.Id == id);
    }
}
