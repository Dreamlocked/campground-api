namespace campground_api.Models.Dto
{
    public class ImageDto
    {
        public int? Id { get; set; }
        public string Url { get; set; } = "";
        public string Filename { get; set; } = "";
    }
}
