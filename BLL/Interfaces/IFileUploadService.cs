using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(Stream stream, string bucketName, string key);
    }
}
