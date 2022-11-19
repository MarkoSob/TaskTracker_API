using Microsoft.EntityFrameworkCore;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.BasicGenericRepository;

namespace TaskTracker_DAL.GenericRepository
{
    public class GenericRepository<T> : BasicGenericRepository<T> , IGenericRepository<T> where T : Entity, new()
    {
        public GenericRepository(EfDbContext dbContext) :base(dbContext)
        {}

        public override async Task<T> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            return await base.CreateAsync(entity);
        }

        public async Task<T> GetByIdAsync(Guid id) 
            => await _dbSet.SingleOrDefaultAsync(x => x.Id == id);

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
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
