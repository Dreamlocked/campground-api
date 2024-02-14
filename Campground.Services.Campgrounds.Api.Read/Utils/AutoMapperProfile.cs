using AutoMapper;
using Campground.Services.Campgrounds.Api.Read.Querys.Campgrounds.Common;
using Campground.Services.Campgrounds.Domain.Entities;

namespace Campground.Services.Campgrounds.Api.Write.Utils
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Domain.Entities.Campground, CampgroundResponse>()
                .ForMember(d => d.Reviews, opt => opt.MapFrom(o => o.Bookings.Select(b => b.Review)))
                .ForMember(d => d.Reviews.Tennant, opt => opt.MapFrom(o => o.Bookings.Select(b => b.User)));
                
        }
    }
}
