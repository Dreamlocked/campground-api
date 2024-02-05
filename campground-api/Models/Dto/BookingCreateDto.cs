namespace campground_api.Models.Dto
{
    public class BookingCreateDto
    {
        public required int CampgroundId { get; set; }
        public required DateOnly ArrivingDate { get; set; }
        public required DateOnly LeavingDate { get; set; }
        public required int NumNights { get; set; }
        public required decimal PricePerNight { get; set; }
    }
}
