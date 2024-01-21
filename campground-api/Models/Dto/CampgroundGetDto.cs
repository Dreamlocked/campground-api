namespace campground_api.Models.Dto
{
    public class CampgroundGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public Double Score { get; set; }
        public List<ImageDto>? Images { get; set; }
        public UserDto? Host { get; set; }
        public List<ReviewListDto>? Reviews { get; set; }
    }
}
