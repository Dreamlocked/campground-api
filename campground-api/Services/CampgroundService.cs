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
            Campground? newCampground = null;
            try
            {
                newCampground = new Campground()
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

        public async Task<Campground?> Delete(int id)
        {
            var campground = await _context.Campgrounds.FindAsync(id);
            if(campground == null)
            {
                return null;
            }

            _context.Campgrounds.Remove(campground);
            await _context.SaveChangesAsync();

            return campground;
        }

        public async Task<CampgroundGetDto?> Update(int id, CampgroundCreateDto campgroundDto)
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

            var imagesToDelete = await _context.Images.Where(image => image.CampgroundId == campground.Id).ToListAsync();

            _context.Images.RemoveRange(imagesToDelete);

            await UploadImageCampground(campground.Id, campgroundDto.Images);

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

                var contentType = GetContentType(fileExtension);

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
