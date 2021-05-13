using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;

namespace FootballSubscriber.Core.Interfaces
{
    public interface ISubscriptionService
    {
        /// <summary>
        /// Creates a new subscription fr
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Subscription> CreateSubscriptionAsync(int teamId, string userId);
    }
}