namespace Campground.Services.Campgrounds.Api.Read.Querys.Users.Common
{
    public record UserResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email);
}
