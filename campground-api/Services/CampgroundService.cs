using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using campground_api.Models;
using campground_api.Models.Dto;
using campground_api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using System.Net.Http.Headers;

namespace campground_api.Services
{
    public class CampgroundService(CampgroundContext context, BlobServiceClient blobServiceClient)
    {
        private readonly CampgroundContext _context = context;
        private readonly BlobServiceClient _blobServiceClient = blobServiceClient;

        public async Task<List<CampgroundListDto>> GetAll()
        {
            var campgrounds = await _context.Campgrounds
                .Include(c => c.Images)
                .Include(c => c.Bookings)
                .ThenInclude(b => b.Reviews)
                .ToListAsync();

            return campgrounds.Select(Mapper.MapCampgroundToCampgroundListDto).ToList();
        }

        public async Task<CampgroundGetDto?> Get(int id)
        {
            var campground = await _context.Campgrounds
                .Include(c => c.Images)
                .Include(c => c.Bookings)
                .ThenInclude(r => r.User)
                .Include(c => c.Bookings)
                .ThenInclude(r => r.Reviews)
                .Include(c => c.Host)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(campground == null) return null;

            return Mapper.MapCampgroundToCampgroundGetDto(campground);
        }
           
        public async Task<CampgroundGetDto?> Create(int userId, CampgroundCreateDto campgroundDto)
        {
            try
            {
                Campground newCampground = new Campground()
                {
                    HostId = userId,
                    Title = campgroundDto.Title,
                    Latitude = campgroundDto.Latitude,
                    Longitude = campgroundDto.Longitude,
                    Price = campgroundDto.Price,
                    Description = campgroundDto.Description,
                    Location = campgroundDto.Location,
                    ProvinceId = campgroundDto.ProvinceId 
                };

                _context.Campgrounds.Add(newCampground);

                await _context.SaveChangesAsync();

                await UploadImageCampground(newCampground.Id, campgroundDto.Images);

                return await Get(newCampground.Id);
            }
            catch(Exception)
            {
                throw;
            }

        }

        public async Task<Campground?> Delete(int userId, int id)
        {
            var campground = await _context.Campgrounds.FindAsync(id) ?? throw new Exception("No existe el campground");

            if(campground.HostId != userId) throw new Exception("El usuario no tiene autorizacion sobre el campgorund");

            if(campground.Bookings.Count > 0) throw new Exception("El campground tiene reservas asociadas");

            await DeleteAllFromCampground(campground.Id);

            _context.Campgrounds.Remove(campground);

            await _context.SaveChangesAsync();

            return campground;
        }

        public async Task<CampgroundGetDto?> Update(int id, CampgroundUpdateDto campgroundDto)
        {
            var campground = await _context.Campgrounds.FindAsync(id);

            if(campground == null) return null;

            campground.HostId = campground.HostId;
            campground.Title = campgroundDto.Title;
            campground.Latitude = campgroundDto.Latitude;
            campground.Longitude = campgroundDto.Longitude;
            campground.Price = campgroundDto.Price;
            campground.Description = campgroundDto.Description;
            campground.Location = campgroundDto.Location;

            await _context.SaveChangesAsync();

            // determinar las imagenes que se eliminarán
            var imagesToDelete = campground.Images.Where(image => !campgroundDto.actualImages.Contains(image.Id)).ToList();

            imagesToDelete.ForEach(async image => await DeleteImageFromCampground(campground.Id, image.Filename));

            _context.Images.RemoveRange(imagesToDelete);

            if(campgroundDto.Images is not null) await UploadImageCampground(campground.Id, campgroundDto.Images);

            await _context.SaveChangesAsync();

            return await this.Get(campground.Id);
        }

        public async Task<List<Image>> UploadImageCampground(int campgroundId, List<IFormFile> images)
        {
            var containerCampground = _blobServiceClient.GetBlobContainerClient($"camp0{campgroundId}");

            await containerCampground.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var newImages = new List<Image>();

            foreach(var image in images)
            {
                var fileExtension = Path.GetExtension(ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName?.Trim('"'));

                var fileName = $"{Guid.NewGuid()}{fileExtension}";

                var contentType = GetContentType(fileExtension!);

                var blobUploadOptions = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    }
                };

                var blobClient = containerCampground.GetBlobClient(fileName);
                using(var stream = image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, blobUploadOptions);
                }

                var newImage = new Image()
                {
                    CampgroundId = campgroundId,
                    Filename = fileName,
                    Url = blobClient.Uri.ToString()
                };

                _context.Images.Add(newImage);
                newImages.Add(newImage);
            }

            await _context.SaveChangesAsync();

            return newImages;
        }

        public async Task DeleteImageFromCampground(int campgroundId, string imageId)
        {
            var containerCampground = _blobServiceClient.GetBlobContainerClient($"camp0{campgroundId}");

            var blobClient = containerCampground.GetBlobClient(imageId);

            await blobClient.DeleteIfExistsAsync();
        }

        public async Task DeleteAllFromCampground(int campgroundId)
        {
            var containerCampground = _blobServiceClient.GetBlobContainerClient($"camp0{campgroundId}");

            await containerCampground.DeleteIfExistsAsync();
        }

        private string GetContentType(string fileExtension)
        {
            // Aquí puedes agregar más tipos de contenido según sea necesario
            return fileExtension switch
            {
                ".jpg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }

    }
}
