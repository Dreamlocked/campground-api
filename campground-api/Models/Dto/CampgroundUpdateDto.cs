namespace campground_api.Models.Dto
{
    public class CampgroundUpdateDto
    {
        public required string Title { get; set; } = "";
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public required int ProvinceId { get; set; } 
        public required List<int> actualImages { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
