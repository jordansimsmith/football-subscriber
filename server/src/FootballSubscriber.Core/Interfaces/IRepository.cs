using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FootballSubscriber.Core.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    ///     Adds an entity to the persistence context
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(TEntity entity);

    /// <summary>
    ///     Adds a range of entities to the persistence context
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    ///     Saves changes to the persistence context
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    ///     Removes the provided entity from the persistence context
    /// </summary>
    /// <param name="entities"></param>
    void Remove(TEntity entities);

    /// <summary>
    ///     Queries the persistence context and returns an ordered set of results
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="includeProperties"></param>
    Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, object>> orderBy,
        params Expression<Func<TEntity, object>>[] includeProperties
    );

    /// <summary>
    ///     Queries the persistence context for the existence of one or more entities
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
}
