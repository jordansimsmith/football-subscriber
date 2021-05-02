using System.Collections.Generic;
using System.Threading.Tasks;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Infrastructure.Data
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity: class
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
    }
}