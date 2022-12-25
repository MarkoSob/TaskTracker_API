using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TaskTracker.Core.Extensions;
using TaskTracker.Core.QueryParameters;

namespace TaskTracker_DAL.BasicGenericRepository
{
    public class BasicGenericRepository<T> : IBasicGenericRepository<T> where T : class, new()
    {
        protected readonly EfDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<BasicGenericRepository<T>> _logger;

        public BasicGenericRepository(EfDbContext dbContext, ILogger<BasicGenericRepository<T>> logger)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _logger = logger;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public IQueryable<T> GetByPredicate(Expression<Func<T, bool>> expression)
            =>  _dbSet.Where(expression);

        public async Task<IEnumerable<T>> GetAllAsync()
          => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<bool> DeleteAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;

            return await _dbContext.SaveChangesAsync() != 0;
        }
    }
}
