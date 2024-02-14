using MediatR;

namespace Campground.Services.Campgrounds.Consumer.Users.Create
{
    public record CreateUserCommand(
        string Username,
        string FirstName,
        string LastName,
        string Password,
        string Email,
        string UrlPhoto
    ) : IRequest<Unit>;
}
