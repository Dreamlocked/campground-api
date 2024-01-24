namespace campground_api.Models.Dto
{
    public class CampgroundUpdateDto
    {
        public string Title { get; set; } = "";
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public int ProvinceId { get; set; } 
        public List<int> actualImages { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
