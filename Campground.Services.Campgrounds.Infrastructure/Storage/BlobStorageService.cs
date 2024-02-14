using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Services.Campgrounds.Infrastructure.Storage
{
    public class BlobStorageService(BlobServiceClient blobServiceClient) : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient = blobServiceClient;

        public async Task<string> UploadFileAsync(string containerName, string fileName, byte[] fileContent)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = container.GetBlobClient(fileName);
            using(var stream = new MemoryStream(fileContent))
            {
                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = GetContentType(Path.GetExtension(fileName))
                    }
                });
            }

            return blobClient.Uri.ToString();
        }

        public async Task<byte[]> DownloadFileAsync(string containerName, string fileName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = container.GetBlobClient(fileName);
            var response = await blobClient.DownloadAsync();
            using var memoryStream = new MemoryStream();
            await response.Value.Content.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task DeleteFileAsync(string containerName, string fileName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = container.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }

        private string GetContentType(string fileExtension)
        {
            return fileExtension switch
            {
                ".jpg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }
    }
}
