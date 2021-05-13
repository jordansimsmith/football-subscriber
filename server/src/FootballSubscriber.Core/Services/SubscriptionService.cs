using System;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FootballSubscriber.Core.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ILogger<Subscription> _logger;
        private readonly IRepository<Subscription> _subscriptionRepository;

        public SubscriptionService(IRepository<Subscription> subscriptionRepository, ILogger<Subscription> logger)
        {
            _subscriptionRepository = subscriptionRepository;
            _logger = logger;
        }

        public async Task<Subscription> CreateSubscriptionAsync(int teamId, string userId)
        {
            if (await _subscriptionRepository.AnyAsync(s => s.TeamId == teamId && s.UserId == userId))
                throw new SystemException("A subscription already exists for this team");

            var subscription = new Subscription
            {
                TeamId = teamId,
                UserId = userId
            };

            try
            {
                await _subscriptionRepository.AddAsync(subscription);
                await _subscriptionRepository.SaveChangesAsync();

                return subscription;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new SystemException("An error was encountered when creating the subscription");
            }
        }
    }
}