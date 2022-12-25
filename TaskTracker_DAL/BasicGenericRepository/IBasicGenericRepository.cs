using System.Linq.Expressions;
using TaskTracker.Core.QueryParameters;

namespace TaskTracker_DAL.BasicGenericRepository
{
    public interface IBasicGenericRepository<T> where T:class, new()
    {
        Task<T> CreateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetByPredicate(Expression<Func<T, bool>> expression);
    }
}
