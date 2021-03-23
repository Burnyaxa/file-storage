using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IStatisticsService
    {
        Task IncreaseViews(FileDto fileDto);
        Task IncreaseDownloads(FileDto fileDto);
    }
}
