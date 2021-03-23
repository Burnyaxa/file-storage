using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> UploadFileAsync(FileDto file, string bucketName);
        Task DeleteFileAsync(string bucketName, string link);
        string DownloadFile(string bucketName, string link);
    }
}
