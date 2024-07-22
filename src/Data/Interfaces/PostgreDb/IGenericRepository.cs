using Domain.Entities;

namespace Data.Interfaces.PostgreDb
{
    public interface IGenericRepository<T> where T : Entity
    {

        Task<T?> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
