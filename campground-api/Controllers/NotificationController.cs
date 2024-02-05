using campground_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace campground_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController(NotificationService notificationService) : ControllerBase
    {
        private readonly NotificationService _notificationService = notificationService;

        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var notifications = _notificationService.GetAllNotifictionsByUser(int.Parse(userId!));
            return Ok(notifications);
        }

        [HttpPost("markviewed/{id}")]
        public IActionResult Post(int id)
        {
            var notification = _notificationService.MarkAsViewed(id);
            return Ok(notification);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            _notificationService.CleanAllNotificationsByUser(int.Parse(userId!));
            return Ok();
        }
    }
}
