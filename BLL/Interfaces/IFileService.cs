using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IFileService
    {
        Task<FileDto> CreateFileAsync(FileDto fileDto, string token);
        Task<IEnumerable<FileDto>> GetAllFilesAsync(string name, string token);
        Task<FileDto> GetFileByIdAsync(int id);
        Task<FileDto> GetFileByShortLinkAsync(string shortLink);
        Task<FileDto> UpdateFileAsync(int id, FileDto fileDto, string token);
        Task<IEnumerable<FileDto>> GetAllFilesByUserIdAsync(int id, string name);
        Task DeleteFileAsync(int id, string token);
    }
}
