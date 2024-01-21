namespace campground_api.Models.Dto
{
    public class CampgroundListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public List<ImageDto>? Images { get; set; }
        public Double? Score { get; set; }
    }
}
