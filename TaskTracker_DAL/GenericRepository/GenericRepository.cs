using Microsoft.EntityFrameworkCore;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.BasicGenericRepository;
using Microsoft.Extensions.Logging;

namespace TaskTracker_DAL.GenericRepository
{
    public class GenericRepository<T> : BasicGenericRepository<T> , IGenericRepository<T> where T : Entity, new()
    {
        public GenericRepository(EfDbContext dbContext, ILogger<GenericRepository<T>> logger) :base(dbContext, logger)
        {}

        public override async Task<T> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            return await base.CreateAsync(entity);
        }

        public async Task<T> GetByIdAsync(Guid id) 
            => await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(Guid id)
        {
            var entity = new T { Id = id };
            _dbContext.Entry(entity).State = EntityState.Deleted;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                _logger.LogError($"Can't delete task with id {id} as it doesn't exist");
            }
            return entity;
        }
    }
}
