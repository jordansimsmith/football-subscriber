using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballSubscriber.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adds an entity to the persistence context
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Adds a range of entities to the persistence context
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Saves changes to the persistence context
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}