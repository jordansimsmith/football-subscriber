using System.Collections.Generic;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;

namespace FootballSubscriber.Core.Interfaces;

public interface ISubscriptionService
{
    /// <summary>
    ///     Creates a new subscription fr
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Subscription> CreateSubscriptionAsync(int teamId, string userId);

    /// <summary>
    ///     Gets the subscriptions for the current user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<Subscription>> GetSubscriptionsAsync(string userId);

    /// <summary>
    ///     Deletes a subscription created by the user
    /// </summary>
    /// <param name="subscriptionId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteSubscriptionAsync(int subscriptionId, string userId);
}