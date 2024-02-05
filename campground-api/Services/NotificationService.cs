using campground_api.Models;
using campground_api.Models.Dto;
using campground_api.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace campground_api.Services
{
    public class NotificationService(CampgroundContext context)
    {
        private readonly CampgroundContext _context = context;

        public List<NotificationGetDto> GetAllNotifictionsByUser(int userId)
        {
            var notifications = _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreateAt)
                .Select(Mapper.MapNotificationToNotificationGet)
                .ToList();

            return notifications;
        }

        public NotificationGetDto? MarkAsViewed(int notificationId)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.Id == notificationId);

            if(notification == null) return null;

            notification.Viewed = true;

            _context.SaveChanges();

            return Mapper.MapNotificationToNotificationGet(notification);
        }

        public void  CleanAllNotificationsByUser(int userId)
        {
            try
            {
                var notifications = _context.Notifications.Where(n => n.UserId == userId).ToList();
                _context.Notifications.RemoveRange(notifications);
                _context.SaveChanges();
            }catch(Exception ex)
            {
                   Console.WriteLine(ex.Message);
            }
        }

        public async Task<NotificationGetDto> CreateNotification(int tenantId, int campgroundId)
        {
            var campground = await _context.Campgrounds.FirstOrDefaultAsync(campground => campground.Id == campgroundId);

            var tenant = await _context.Users.FirstOrDefaultAsync(u => u.Id == tenantId);

            var newNotificaction = new Notification()
            {
                UserId = campground!.HostId,
                Message = $"{tenant!.FirstName} made a booking of {campground.Title}",
                BookingId = campgroundId,
                CreateAt = DateTime.Now,
                Viewed = false
            };

            _context.Notifications.Add(newNotificaction);

            await _context.SaveChangesAsync();

            return Mapper.MapNotificationToNotificationGet(newNotificaction); ;
        }
    }
}
