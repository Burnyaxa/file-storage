using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class FileService : IFileService
    {
        private readonly UserManager<User> _manager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtFactory _factory;
        private readonly IMapper _mapper;
        private const string IncludeProperties = "User,UserId,StatisticsId,Statistics";

        public FileService(IUnitOfWork unitOfWork, IMapper mapper, IJwtFactory factory, UserManager<User> manager)
        {
            _manager = manager;
            _factory = factory;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<FileDto> CreateFileAsync(FileDto fileDto, string token)
        {
            if (fileDto == null)
            {
                throw new ArgumentNullException(nameof(fileDto));
            }

            var ownerId = _factory.GetUserIdClaim(token);

            var entity = _mapper.Map<FileDto, File>(fileDto);
            var statistics = _mapper.Map<FileDto, FileStatistics>(fileDto);

            entity.UserId = ownerId;

            var result = _unitOfWork.FileRepository.Add(entity);
            _unitOfWork.FileStatisticsRepository.Add(statistics);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<File, FileDto>(result);
        }

        public async Task<IEnumerable<FileDto>> GetAllFilesAsync(string name, string token)
        {
            if (!CheckSearchRights(token))
            {
                throw new NotEnoughRightsException();
            }

            var files = await _unitOfWork.FileRepository
                .GetAllWithDetails(IncludeProperties)
                .ToListAsync();

            if (name != string.Empty)
            {
                files = files.Where(x => x.Name == name).ToList();
            }

            if (!files.Any())
            {
                throw new EntityCollectionNotFoundException(nameof(files));
            }

            return _mapper.Map<IEnumerable<File>, IEnumerable<FileDto>>(files);
        }

        public async Task<FileDto> GetFileByIdAsync(int id)
        {
            var file = await _unitOfWork.FileRepository
                .GetByIdWithDetailsAsync(id, IncludeProperties);

            if (file == null)
            {
                throw new EntityNotFoundException(nameof(file), id);
            }

            return _mapper.Map<File, FileDto>(file);
        }

        public async Task<FileDto> GetFileByShortLinkAsync(string shortLink)
        {
            var file = await _unitOfWork.FileRepository
                .GetAllWithDetails()
                .Where(f => f.ShortUrl == shortLink)
                .FirstOrDefaultAsync();

            if (file == null)
            {
                throw new FileNotFoundException(shortLink);
            }

            if (!file.IsShared)
            {
                throw new NotEnoughRightsException();
            }

            return _mapper.Map<File, FileDto>(file);
        }

        public async Task<FileDto> UpdateFileAsync(int id, FileDto fileDto, string token)
        {
            var file = await _unitOfWork.FileRepository
                .GetByIdWithDetailsAsync(id, IncludeProperties);
            if (file == null)
            {
                throw new EntityNotFoundException(nameof(file), id);
            }

            if (!CheckEditRights(token, file.UserId))
            {
                throw new NotEnoughRightsException();
            }

            if (file.Name != fileDto.Name && fileDto.Name != null)
            {
                file.Name = fileDto.Name;
            }

            if (file.IsShared != fileDto.IsShared)
            {
                file.IsShared = fileDto.IsShared;
            }

            file.LastUpdated = DateTime.Now;

            var result = _unitOfWork.FileRepository.Update(file);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<File, FileDto>(result);
        }

        public async Task<IEnumerable<FileDto>> GetAllFilesByUserIdAsync(int id, string name)
        {
            var user = await _manager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user), id);
            }

            var files = await _unitOfWork.FileRepository
                .GetAllWithDetails(IncludeProperties)
                .Where(f => f.UserId == id)
                .ToListAsync();

            if (name != null)
            {
                files = files.Where(f => f.Name == name).ToList();
            }

            if (!files.Any())
            {
                throw new EntityCollectionNotFoundException(nameof(files));
            }

            return _mapper.Map<IEnumerable<File>, IEnumerable<FileDto>>(files);
        }

        public async Task DeleteFileAsync(int id, string token)
        {
            var file = await _unitOfWork.FileRepository.GetByIdAsync(id);
            if (file == null)
            {
                throw new EntityNotFoundException(nameof(file), id);
            }

            if (!CheckEditRights(token, id))
            {
                throw new NotEnoughRightsException();
            }

            _unitOfWork.FileRepository.Delete(file);
            await _unitOfWork.SaveAsync();
        }

        private bool CheckEditRights(string token, int id)
        {
            var claimsId = _factory.GetUserIdClaim(token);
            var role = _factory.GetUserRoleClaim(token);
            return role == "Administrator" || claimsId.Equals(id);
        }

        private bool CheckSearchRights(string token)
        {
            var role = _factory.GetUserRoleClaim(token);
            return role == "Administrator";
        }
    }
}
