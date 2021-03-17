using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity<int>
    {
        public TEntity Add(TEntity entity);
        public TEntity Update(TEntity entity);
        public void Delete(TEntity entity);
        public Task<TEntity> GetByIdAsync(int id);
        public Task<TEntity> GetByIdWithDetailsAsync(int id);
        public Task<TEntity> GetAllAsync();
        public Task<TEntity> GetAllWithDetailsAsync();
    }
}
