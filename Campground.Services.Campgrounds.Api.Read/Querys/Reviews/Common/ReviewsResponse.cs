using Campground.Services.Campgrounds.Api.Read.Querys.Users.Common;

namespace Campground.Services.Campgrounds.Api.Read.Querys.Reviews.Common
{
    public record ReviewsResponse(
        Guid Id,
        UserResponse Tennant,
        string Comment,
        int Rating,
        DateTime CreateAt
        );
}
