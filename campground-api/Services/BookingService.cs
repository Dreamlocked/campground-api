using Azure.Messaging.ServiceBus;
using campground_api.Models;
using campground_api.Models.Dto;
using campground_api.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ms_correo.Models;
using Newtonsoft.Json;

namespace campground_api.Services
{
    public class BookingService(CampgroundContext context)
    {
        private readonly CampgroundContext _context = context;

        public async Task<List<BookingGetDto>> GetAll(int userId)
        {
            var bookings = await _context.Bookings
                .Include(x => x.User)
                .Include(x => x.Campground)
                .ThenInclude(c => c.Images)
                .Include(x => x.Campground)
                .ThenInclude(c => c.Host)
                .Include(x => x.Reviews)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return bookings.Select(Mapper.MapBookingToBookingGet).ToList();
        }

        public async Task<BookingGetDto?> Get(int id)
        {
            var booking = await _context.Bookings
                .Include(x => x.User)
                .Include(x => x.Campground)
                .ThenInclude(c => c.Images)
                .Include(x => x.Campground)
                .ThenInclude(c => c.Host)
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(booking == null) return null;

            return Mapper.MapBookingToBookingGet(booking);
        }

        public async Task<BookingGetDto?> Create(int userId, BookingCreateDto bookingCreateDto)
        {
            try
            {
                var newBooking = new Booking()
                {
                    UserId = userId,
                    CampgroundId = bookingCreateDto.CampgroundId,
                    ArrivingDate = bookingCreateDto.ArrivingDate,
                    LeavingDate = bookingCreateDto.LeavingDate,
                    NumNights = bookingCreateDto.NumNights,
                    PricePerNight = bookingCreateDto.PricePerNight
                };

                _context.Bookings.Add(newBooking);

                await _context.SaveChangesAsync();

                return await Get(newBooking.Id);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}