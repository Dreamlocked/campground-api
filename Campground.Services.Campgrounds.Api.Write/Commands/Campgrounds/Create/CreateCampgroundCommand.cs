using MediatR;

namespace Campground.Services.Campgrounds.Api.Write.Commands.Campgrounds.Create
{
    public record CreateCampgroundCommand(
        string Title,
        decimal Latitude,
        decimal Longitude,
        decimal PricePerNight,
        string Description,
        string Location,
        List<IFormFile> Images
    ) : IRequest<Unit>;
}
