using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string bucketName, string folder);
        Task DeleteFileAsync(string bucketName, string link);
    }
}
