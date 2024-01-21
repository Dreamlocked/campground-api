namespace campground_api.Models.Dto
{
    public class ReviewCreateDto
    {
        public int BookingsId { get; set; }
        public string Body { get; set; } = "";
        public short Rating { get; set; }
    }
}
