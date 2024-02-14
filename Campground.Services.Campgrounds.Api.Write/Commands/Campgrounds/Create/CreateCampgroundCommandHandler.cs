using AutoMapper;
using Campground.Services.Campgrounds.Infrastructure.Data.Unit_of_Work;
using Campground.Services.Campgrounds.Domain.Entities;
using Campground.Services.Campgrounds.Infrastructure.Storage;
using MediatR;
using System.Security.Claims;

namespace Campground.Services.Campgrounds.Api.Write.Commands.Campgrounds.Create
{
    internal sealed class CreateCampgroundCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IBlobStorageService blobStorageService) : IRequestHandler<CreateCampgroundCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;

        public async Task<Unit> Handle(CreateCampgroundCommand request, CancellationToken cancellationToken)
        {
            var campground = _mapper.Map<CreateCampgroundCommand, Domain.Entities.Campground>(request);
            campground.Id = Guid.NewGuid();
            campground.HostId = Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var imageTasks = request.Images.Select(async image =>
            {
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                var newImage = new Image()
                {
                    Id = Guid.NewGuid(),
                    CampgroundsId = campground.Id,
                    Filename = image.FileName,
                    Url = await _blobStorageService.UploadFileAsync(campground.Id.ToString().Split('-')[0], image.FileName, memoryStream.ToArray())
                };
                return newImage;
            });

            campground.Images = (await Task.WhenAll(imageTasks)).ToList();

            await _unitOfWork.CampgroundRepository.AddAsync(campground);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
