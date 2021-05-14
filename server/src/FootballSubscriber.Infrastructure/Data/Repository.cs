using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FootballSubscriber.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FootballSubscriber.Infrastructure.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly FootballSubscriberContext _dbContext;

        public Repository(FootballSubscriberContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.AddRangeAsync(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Remove(TEntity entities)
        {
            _dbContext.Remove(entities);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, object>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _dbContext.Set<TEntity>().Where(filter);
            if (includeProperties != null && includeProperties.Any())
            {
                query = includeProperties.Aggregate(query, (current, property) => current.Include(property));
            }

            query = query.OrderBy(orderBy);
            return await query.ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(filter);
        }
    }
}