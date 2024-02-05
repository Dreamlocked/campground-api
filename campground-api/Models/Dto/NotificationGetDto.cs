namespace campground_api.Models.Dto
{
    public class NotificationGetDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? BookingId { get; set; } 

        public string Message { get; set; } = null!;

        public DateTime? CreateAt { get; set; }

        public bool? Viewed { get; set; }

    }

}
