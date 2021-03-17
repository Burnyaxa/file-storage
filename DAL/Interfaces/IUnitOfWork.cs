using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        public IRepository<File> FileRepository { get; }
        public IRepository<FileStatistics> FileStatisticsRepository { get; }

        public Task SaveAsync();
    }
}
