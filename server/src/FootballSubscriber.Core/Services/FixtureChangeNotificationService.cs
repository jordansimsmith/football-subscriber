using System.Linq;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Models;
using Microsoft.Extensions.Logging;

namespace FootballSubscriber.Core.Services;

public class FixtureChangeNotificationService : IFixtureChangeNotificationService
{
    private readonly IEmailService _emailService;
    private readonly ILogger<FixtureChangeNotificationService> _logger;
    private readonly IRepository<Subscription> _subscriptionRepository;
    private readonly IUserProfileService _userProfileService;

    public FixtureChangeNotificationService(
        IEmailService emailService,
        IRepository<Subscription> subscriptionRepository,
        IUserProfileService userProfileService,
        ILogger<FixtureChangeNotificationService> logger
    )
    {
        _emailService = emailService;
        _subscriptionRepository = subscriptionRepository;
        _userProfileService = userProfileService;
        _logger = logger;
    }

    public async Task NotifySubscribersAsync(Fixture oldFixture, Fixture newFixture)
    {
        var subscriptions = (
            await _subscriptionRepository.FindAsync(
                s => s.TeamId == oldFixture.HomeTeamId || s.TeamId == oldFixture.AwayTeamId,
                s => s.Id
            )
        ).ToList();

        if (!subscriptions.Any())
        {
            _logger.LogInformation(
                "Fixture {newFixture.Id} changed but there were no subscribers to listen to it",
                newFixture.Id
            );
            return;
        }

        var userIds = subscriptions.Select(s => s.UserId).Distinct();

        _logger.LogInformation("Fetching subscriber user profiles");
        var userProfiles = await Task.WhenAll(
            userIds.Select(u => _userProfileService.GetUserProfileAsync(u))
        );
        _logger.LogInformation("Successfully fetched subscriber user profiles");

        var fixtureChange = new FixtureChangeModel
        {
            AwayTeam = newFixture.AwayTeamName,
            HomeTeam = newFixture.HomeTeamName,
            NewAddress = newFixture.Address,
            NewDateTime = newFixture.Date,
            NewVenue = newFixture.VenueName,
            OldAddress = oldFixture.Address,
            OldDateTime = oldFixture.Date,
            OldVenue = oldFixture.VenueName
        };

        _logger.LogInformation("Sending fixture change emails to subscribers");
        await Task.WhenAll(
            userProfiles.Select(
                user => _emailService.SendFixtureChangeEmailAsync(user, fixtureChange)
            )
        );
        _logger.LogInformation("Successfully sent fixture change emails to subscribers");
    }
}
