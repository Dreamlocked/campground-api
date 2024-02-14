using Campground.Services.Campgrounds.Api.Read.Querys.Users.Common;
using MediatR;

namespace Campground.Services.Campgrounds.Api.Read.Querys.Users.GetAll
{
    public record GetAllUsersQuery : IRequest<IReadOnlyList<UserResponse>>;
}
