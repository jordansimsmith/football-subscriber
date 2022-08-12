using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Models;
using FootballSubscriber.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FootballSubscriber.Test.Core;

public class FixtureChangeNotificationServiceTest
{
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IRepository<Subscription>> _mockSubscriptionRepository;
    private readonly Mock<IUserProfileService> _mockUserProfileService;

    private readonly FixtureChangeNotificationService _subject;

    public FixtureChangeNotificationServiceTest()
    {
        _mockEmailService = new Mock<IEmailService>();
        _mockSubscriptionRepository = new Mock<IRepository<Subscription>>();
        _mockUserProfileService = new Mock<IUserProfileService>();

        _subject = new FixtureChangeNotificationService(
            _mockEmailService.Object,
            _mockSubscriptionRepository.Object,
            _mockUserProfileService.Object,
            Mock.Of<ILogger<FixtureChangeNotificationService>>()
        );
    }

    [Fact]
    public async Task NotifySubscribersAsync_Should_SendEmailNotifications()
    {
        // arrange
        var newFixture = new Fixture();
        var oldFixture = new Fixture { HomeTeamId = 1, AwayTeamId = 2 };

        var subscriptions = new[] { new Subscription { UserId = "user 1" } };

        var userProfile = new UserProfile { Email = "user1@user1.com", Name = "user 1" };

        _mockSubscriptionRepository
            .Setup(
                x =>
                    x.FindAsync(
                        It.IsAny<Expression<Func<Subscription, bool>>>(),
                        It.IsAny<Expression<Func<Subscription, object>>>()
                    )
            )
            .ReturnsAsync(subscriptions)
            .Verifiable();

        _mockUserProfileService
            .Setup(x => x.GetUserProfileAsync("user 1"))
            .ReturnsAsync(userProfile)
            .Verifiable();

        // act
        await _subject.NotifySubscribersAsync(oldFixture, newFixture);

        // assert
        _mockSubscriptionRepository.Verify();
        _mockUserProfileService.Verify();
        _mockEmailService.Verify(
            x => x.SendFixtureChangeEmailAsync(userProfile, It.IsAny<FixtureChangeModel>())
        );
    }
}
