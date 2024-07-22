using Data.Context;
using Data.Interfaces.PostgreDb;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.PostgreDb
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : Entity
    {
        public readonly PostgreDbContext _dbContext;

        public GenericRepository(PostgreDbContext context)
        {
            _dbContext = context;
        }

        public async Task<T?> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

    }
}
