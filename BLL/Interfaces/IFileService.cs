﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IFileService
    {
        Task<FileDto> CreateFileAsync(FileDto fileDto, string token);
        Task<IEnumerable<FileDto>> GetAllFilesAsync();
        Task<FileDto> GetFileByIdAsync(int id);
        Task<FileDto> UpdateFileAsync(int id, FileDto fileDto, string token);
        Task<IEnumerable<FileDto>> GetAllFilesByUserIdAsync(int id);
    }
}