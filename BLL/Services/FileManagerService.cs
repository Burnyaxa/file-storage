using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;

namespace BLL.Services
{
    public class FileManagerService : IFileManagerService
    {
        private readonly IFileService _fileService;
        private readonly IShortLinkService _shortLinkService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IStatisticsService _statisticsService;

        public FileManagerService(IFileService fileService, IShortLinkService shortLinkService,
            ICloudStorageService cloudStorageService, IStatisticsService statisticsService)
        {
            _fileService = fileService;
            _shortLinkService = shortLinkService;
            _cloudStorageService = cloudStorageService;
            _statisticsService = statisticsService;
        }

        public async Task<FileDto> CreateFileAsync(FileDto fileDto, string token, string bucket)
        {
            string url = await _cloudStorageService.UploadFileAsync(fileDto, bucket);
            string shortUrl = _shortLinkService.GenerateShortLink(url);

            fileDto.Url = url;
            fileDto.ShortUrl = shortUrl;

            return await _fileService.CreateFileAsync(fileDto, token);
        }

        public async Task<IEnumerable<FileDto>> GetAllFilesAsync(string name, string token)
        {
            return await _fileService.GetAllFilesAsync(name, token);
        }

        public async Task<FileDto> GetFileByIdAsync(int id)
        {
            await _statisticsService.IncreaseViewsAsync(id);
            return await _fileService.GetFileByIdAsync(id);
        }

        public async Task<FileDto> UpdateFileAsync(int id, FileDto fileDto, string token)
        {
            return await _fileService.UpdateFileAsync(id, fileDto, token);
        }

        public async Task<IEnumerable<FileDto>> GetAllFilesByUserIdAsync(int id, string name)
        {
            return await _fileService.GetAllFilesByUserIdAsync(id, name);
        }

        public async Task DeleteFile(int id, string bucket, string token)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            await _cloudStorageService.DeleteFileAsync(bucket, file.Url);
            await _fileService.DeleteFileAsync(id, token);
        }

        public async Task<string> DownloadFileAsync(string bucket, int id)
        {
            var file = await _fileService.GetFileByIdAsync(id);
            await _statisticsService.IncreaseViewsAsync(id);
            await _statisticsService.IncreaseDownloads(id);
            return _cloudStorageService.DownloadFile(bucket, file.Url);
        }

        public async Task<string> DownloadFileByShortLinkAsync(string bucket, string shortLink)
        {
            var file = await _fileService.GetFileByShortLinkAsync(shortLink);
            await _statisticsService.IncreaseViewsAsync(file.Id);
            await _statisticsService.IncreaseDownloads(file.Id);
            return _cloudStorageService.DownloadFile(bucket, file.Url);
        }
    }
}
