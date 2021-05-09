using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballSubscriber.Core.Interfaces
{
    public interface IMerger<TEntity> where TEntity : class
    {
        /// <summary>
        ///     Merges the collection of old entities and new entities
        /// </summary>
        /// <param name="oldEntities"></param>
        /// <param name="newEntities"></param>
        Task MergeAsync(IList<TEntity> oldEntities, IList<TEntity> newEntities);
    }
}