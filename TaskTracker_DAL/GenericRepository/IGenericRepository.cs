using System.Linq.Expressions;
using TaskTracker_DAL.Entities;

namespace TaskTracker_DAL.GenericRepository
{
    public interface IGenericRepository<T> where T : Entity, new()
    {
        Task<T> CreateAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetByPredicate(Expression<Func<T, bool>> expression);
    }
}
