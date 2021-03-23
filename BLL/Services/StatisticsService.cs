using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;

namespace BLL.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task IncreaseViewsAsync(int id)
        {
            var file = await _unitOfWork.FileRepository.GetByIdWithDetailsAsync(id);
            if (file == null)
            {
                throw new EntityNotFoundException(nameof(file), id);
            }

            file.Statistics.Views++;
            _unitOfWork.FileRepository.Update(file);
            await _unitOfWork.SaveAsync();
        }

        public async Task IncreaseDownloadsAsync(int id)
        {
            var file = await _unitOfWork.FileRepository.GetByIdWithDetailsAsync(id);
            if (file == null)
            {
                throw new EntityNotFoundException(nameof(file), id);
            }

            file.Statistics.Downloads++;
            _unitOfWork.FileRepository.Update(file);
            await _unitOfWork.SaveAsync();
        }
    }
}
