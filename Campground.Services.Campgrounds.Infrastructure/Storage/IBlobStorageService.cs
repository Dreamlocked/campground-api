using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Services.Campgrounds.Infrastructure.Storage
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(string containerName, string fileName, byte[] fileContent);
        Task<byte[]> DownloadFileAsync(string containerName, string fileName);
        Task DeleteFileAsync(string containerName, string fileName);
    }
}
