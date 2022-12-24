using Microsoft.EntityFrameworkCore;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.BasicGenericRepository;
using Microsoft.Extensions.Logging;
using TaskTracker.Core.Extensions;
using TaskTracker.Core.Exceptions.DomainExceptions;
using TaskTracker.Core.Exceptions.DataAccessExceptions;

namespace TaskTracker_DAL.GenericRepository
{
    public class GenericRepository<T> : BasicGenericRepository<T>, IGenericRepository<T> where T : Entity, new()
    {
        public GenericRepository(EfDbContext dbContext, ILogger<GenericRepository<T>> logger) : base(dbContext, logger)
        { }

        public override async Task<T> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            return await base.CreateAsync(entity);
        }

        public async Task<T> GetByIdAsync(Guid? id)
        {
            var entity = await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                _logger.LogMessageAndThrowException($"Can't find object with id {id}", new ObjectNotFoundByIdException(typeof(T), (Guid)id));
            }

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                _logger.LogMessageAndThrowException($"Can't update object", new ObjectNotFoundException(typeof(T)));
            }

            return entity;
        }

        public async Task<T> DeleteAsync(Guid? id)
        {
            var entity = new T { Id = (Guid)id };

            _dbContext.Entry(entity).State = EntityState.Deleted;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                _logger.LogMessageAndThrowException($"Can't delete object with id {id}", new ObjectNotFoundByIdException(typeof(T), (Guid)id));
            }

            return entity;
        }
    }
}
