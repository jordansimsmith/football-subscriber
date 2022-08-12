using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Exceptions;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Models;
using Microsoft.Extensions.Logging;

namespace FootballSubscriber.Core.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ILogger<Subscription> _logger;
    private readonly IRepository<Subscription> _subscriptionRepository;

    public SubscriptionService(
        IRepository<Subscription> subscriptionRepository,
        ILogger<Subscription> logger
    )
    {
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
    }

    public async Task<Subscription> CreateSubscriptionAsync(int teamId, string userId)
    {
        if (await _subscriptionRepository.AnyAsync(s => s.TeamId == teamId && s.UserId == userId))
            throw new ConflictException("A subscription already exists for this team");

        var subscription = new Subscription { TeamId = teamId, UserId = userId };

        try
        {
            await _subscriptionRepository.AddAsync(subscription);
            await _subscriptionRepository.SaveChangesAsync();

            return subscription;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new InternalServerErrorException(
                "An error was encountered when creating the subscription"
            );
        }
    }

    public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(string userId)
    {
        return await _subscriptionRepository.FindAsync(
            s => s.UserId == userId,
            s => s.Id,
            s => s.Team
        );
    }

    public async Task DeleteSubscriptionAsync(int subscriptionId, string userId)
    {
        var subscription = (
            await _subscriptionRepository.FindAsync(
                s => s.Id == subscriptionId && s.UserId == userId,
                s => s.Id
            )
        ).FirstOrDefault();
        if (subscription == null)
            throw new NotFoundException("The subscription could not be found");

        _subscriptionRepository.Remove(subscription);
        await _subscriptionRepository.SaveChangesAsync();
    }
}
