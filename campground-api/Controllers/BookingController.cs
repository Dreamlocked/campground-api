using campground_api.Models;
using campground_api.Models.Dto;
using campground_api.Services;
using campground_api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace campground_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(BookingService bookingService, NotificationService notificationService, IHubContext<MessageHub> hubContext) : ControllerBase
    {
        private readonly BookingService _bookingService = bookingService;
        private readonly NotificationService _notificationService = notificationService;
        private readonly IHubContext<MessageHub> _hubContext = hubContext;

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var booking = await _bookingService.Get(id);

            if(booking == null) return NotFound();

            return Ok(booking);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(BookingCreateDto booking)
        {
            try
            {
                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")!.Value);

                var newBooking = await _bookingService.Create(userId, booking);

                if(newBooking == null) return NotFound();

                var newNotification = await _notificationService.CreateNotification(userId, newBooking.Campground.Id);

                await _hubContext.Clients.All.SendAsync("notification", JsonConvert.SerializeObject(newNotification));
                await _hubContext.Clients.User(Convert.ToString(newBooking.Campground.Host!.Id)).SendAsync("notification", newNotification);

                return Ok(newBooking);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

    }
}
