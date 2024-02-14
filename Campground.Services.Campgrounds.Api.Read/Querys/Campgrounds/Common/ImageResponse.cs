namespace Campground.Services.Campgrounds.Api.Read.Querys.Campgrounds.Common
{
    public record ImagesResponse(
        Guid Id,
        string Filename,
        string Url
        );
}
