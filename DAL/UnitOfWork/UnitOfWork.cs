using System.Threading.Tasks;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        private IRepository<File> _fileRepository;
        private IRepository<FileStatistics> _fileStatisticsRepository;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IRepository<File> FileRepository => 
            _fileRepository ??= new Repository<File>(_context);

        public IRepository<FileStatistics> FileStatisticsRepository =>
            _fileStatisticsRepository ??= new Repository<FileStatistics>(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
