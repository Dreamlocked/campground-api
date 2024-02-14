using Campground.Services.Campgrounds.Api.Read.Querys.Users.Common;
using Campground.Services.Campgrounds.Infrastructure.Data.Repository.Base;
using Campground.Services.Campgrounds.Infrastructure.Data.Unit_of_Work;
using MediatR;

namespace Campground.Services.Campgrounds.Api.Read.Querys.Users.GetAll
{
    internal sealed class GetAllUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllUsersQuery, IReadOnlyList<UserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IReadOnlyList<UserResponse>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            return users.Select(user => new UserResponse(
                user.Id,
                user.FirstName!,
                user.LastName!,
                user.Email!
                )).ToList();
        }
    }
}
