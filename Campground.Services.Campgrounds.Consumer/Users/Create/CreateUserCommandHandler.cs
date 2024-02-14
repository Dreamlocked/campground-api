using Campground.Services.Campgrounds.Domain.Entities;
using Campground.Services.Campgrounds.Domain.Utils;
using Campground.Services.Campgrounds.Infrastructure.Data.Repository;
using Campground.Services.Campgrounds.Infrastructure.Data.Unit_of_Work;
using MediatR;

namespace Campground.Services.Campgrounds.Consumer.Users.Create
{
    internal sealed class CreateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = command.Username,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Salt = Encript.GenerateSalt(),
                Email = command.Email,
                UrlPhoto = command.UrlPhoto
            };

            user.Password = Encript.GetSHA256Hash(command.Password + user.Salt);

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
