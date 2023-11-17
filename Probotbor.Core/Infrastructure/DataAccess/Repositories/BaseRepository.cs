using Microsoft.EntityFrameworkCore;
using Probotbor.Core.Contracts.DataAccess;
using System.Linq.Expressions;

namespace Probotbor.Core.Infrastructure.DataAccess.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : EntityCommon
    {
        private readonly ApplicationContext _dbContext;

        public BaseRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id);            
            return entity;
        }

        public async Task<T?> GetFirstWhere(Expression<Func<T, bool>> predicate)
        {
            var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);

            return entity;
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            var list = await _dbContext.Set<T>()
                .Where(predicate)
                .ToListAsync();
            return list;
        }

        public async Task<T?> GetLastWhere<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderExp)
        {
            return await _dbContext.Set<T>()
                .OrderBy(orderExp)
                .LastOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> InitAsync(IEnumerable<T> initCollection, int requiredCount)
        {
            var entities = await _dbContext.Set<T>().ToListAsync();
            if (entities.Count < requiredCount)
            {
                await _dbContext.Set<T>().AddRangeAsync(initCollection);
                await _dbContext.SaveChangesAsync();
                return initCollection;
            }
            return entities;
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>()
                .ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
