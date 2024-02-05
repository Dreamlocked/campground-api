namespace campground_api.Models.Dto
{
    public class BookingGetDto
    {
        public int Id { get; set; }
        public UserDto Tenant { get; set; } = null!;
        public CampgroundGetDto Campground { get; set; } = null!;
        public DateOnly ArrivingDate { get; set; }
        public DateOnly LeavingDate { get; set; }
        public int NumNights { get; set; }
        public decimal? Total { get; set; }
        public List<ReviewListDto>? Reviews { get; set; }
        
    }
}
