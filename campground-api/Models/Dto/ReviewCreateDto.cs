namespace campground_api.Models.Dto
{
    public class ReviewCreateDto
    {
        public required int BookingsId { get; set; }
        public required string Body { get; set; }
        public required short Rating { get; set; }
    }
}
