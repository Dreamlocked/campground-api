using AutoMapper;
using Campground.Services.Campgrounds.Api.Read.Querys.Campgrounds.Common;
using Campground.Services.Campgrounds.Api.Read.Querys.Users.Common;
using Campground.Services.Campgrounds.Api.Read.Querys.Users.GetAll;
using Campground.Services.Campgrounds.Domain.Entities;
using Campground.Services.Campgrounds.Infrastructure.Data.Unit_of_Work;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Campground.Services.Campgrounds.Api.Read.Querys.Campgrounds.GetById
{
    internal sealed class GetCampgroundByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetCampgroundByIdQuery, CampgroundResponse>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CampgroundResponse> Handle(GetCampgroundByIdQuery query, CancellationToken cancellationToken)
        {
            var campground = await _unitOfWork.CampgroundRepository.GetByIdWithDetails(query.Id);

            return _mapper.Map<Domain.Entities.Campground, CampgroundResponse>(campground);
    
        }
    }
}
