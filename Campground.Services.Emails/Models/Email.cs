namespace Campground.Services.Emails.Models
{
    public record Email
    {
        public required string To { get; init; }
        public required string Subject { get; init; }
        public required string Body { get; init; }
    }
}
