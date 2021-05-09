using System.Collections.Generic;
using System.Threading.Tasks;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Services
{
    public abstract class MergerBase<TEntity> : IMerger<TEntity> where TEntity : class
    {
        public async Task MergeAsync(IList<TEntity> oldEntities, IList<TEntity> newEntities)
        {
            var i = 0; // old entity counter
            var j = 0; // new competition counter

            // while there are remaining new and old entities
            while (i < oldEntities.Count && j < newEntities.Count)
            {
                // entity exists in both old and new collections - UPDATE
                if (GetEntityComparableKey(oldEntities[i]) == GetEntityComparableKey(newEntities[j]))
                {
                    await UpdateEntityAsync(oldEntities[i], newEntities[j]);

                    i++;
                    j++;
                    continue;
                }

                // entity exists in new but not in old - INSERT
                if (GetEntityComparableKey(oldEntities[i]) > GetEntityComparableKey(newEntities[j]))
                {
                    await InsertEntityAsync(newEntities[j]);

                    j++;
                    continue;
                }

                // exists in old but not in new - DELETE
                if (GetEntityComparableKey(oldEntities[i]) < GetEntityComparableKey(newEntities[j]))
                {
                    await RemoveEntityAsync(oldEntities[i]);

                    i++;
                }
            }

            // while there are remaining old entities
            while (i < oldEntities.Count)
            {
                await RemoveEntityAsync(oldEntities[i]);

                i++;
            }

            // while there are remaining new entities
            while (j < newEntities.Count)
            {
                await InsertEntityAsync(newEntities[j]);

                j++;
            }

            await OnMergeCompleteAsync();
        }

        protected abstract int GetEntityComparableKey(TEntity entity);

        protected abstract Task UpdateEntityAsync(TEntity oldEntity, TEntity newEntity);

        protected abstract Task InsertEntityAsync(TEntity newEntity);

        protected abstract Task RemoveEntityAsync(TEntity oldEntity);

        protected abstract Task OnMergeCompleteAsync();
    }
}