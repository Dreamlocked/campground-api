using MediatR;

namespace Campground.Services.Campgrounds.Api.Write.Commands.Users.Create
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
