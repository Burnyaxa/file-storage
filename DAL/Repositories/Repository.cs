using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity<int>
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _set;

        public Repository(DbContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            return _set.Add(entity).Entity;
        }

        public TEntity Update(TEntity entity)
        {
            _set.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _set.Attach(entity);
            }
            _set.Remove(entity);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _set.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity> GetByIdWithDetailsAsync(int id, string includeProperties = "")
        {
            return await GetAllWithDetails(includeProperties).FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _set.AsQueryable();
        }

        public IQueryable<TEntity> GetAllWithDetails(string includeProperties = "")
        {
            return includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate<string, IQueryable<TEntity>>(_set,
                    (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
