using AutoMapper;
using Campground.Services.Campgrounds.Api.Write.Commands.Campgrounds.Create;
using Campground.Services.Campgrounds.Domain.Entities;

namespace Campground.Services.Campgrounds.Api.Write.Utils
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<CreateCampgroundCommand, Domain.Entities.Campground>()
                .ForMember(d => d.Images, opt => opt.Ignore());
        }
    }
}
