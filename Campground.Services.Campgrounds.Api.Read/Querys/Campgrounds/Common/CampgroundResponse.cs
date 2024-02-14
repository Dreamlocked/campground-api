using Campground.Services.Campgrounds.Api.Read.Querys.Reviews.Common;
using Campground.Services.Campgrounds.Api.Read.Querys.Users.Common;

namespace Campground.Services.Campgrounds.Api.Read.Querys.Campgrounds.Common
{
    public record CampgroundResponse(
        Guid Id,
        string Title,
        decimal Latitude,
        decimal Longitude,
        decimal PricePerNight,
        string Description,
        string Location,
        DateTime CreateAt,
        UserResponse Host,
        ImagesResponse Images,
        ReviewsResponse Reviews
        );


}
