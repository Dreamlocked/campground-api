using Campground.Services.Campgrounds.Api.Read.Querys.Campgrounds.Common;
using Campground.Services.Campgrounds.Api.Read.Querys.Users.Common;
using MediatR;

namespace Campground.Services.Campgrounds.Api.Read.Querys.Campgrounds.GetById
{
    public record GetCampgroundByIdQuery(Guid Id) : IRequest<CampgroundResponse>;

}
