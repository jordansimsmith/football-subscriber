using System;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Services;

public class FixtureMerger : MergerBase<Fixture>
{
    private readonly IFixtureChangeNotificationService _fixtureChangeNotificationService;
    private readonly IRepository<Fixture> _fixtureRepository;

    public FixtureMerger(
        IRepository<Fixture> fixtureRepository,
        IFixtureChangeNotificationService fixtureChangeNotificationService
    )
    {
        _fixtureRepository = fixtureRepository;
        _fixtureChangeNotificationService = fixtureChangeNotificationService;
    }

    protected override long GetEntityComparableKey(Fixture entity)
    {
        return entity.ApiId;
    }

    protected override async Task UpdateEntityAsync(Fixture oldFixture, Fixture newFixture)
    {
        // important changes to the fixture
        if (
            newFixture.Date < DateTime.UtcNow.AddDays(7)
            && newFixture.Date > DateTime.UtcNow
            && (oldFixture.Date != newFixture.Date || oldFixture.VenueName != newFixture.VenueName)
        )
        {
            // notify subscribers
            await _fixtureChangeNotificationService.NotifySubscribersAsync(oldFixture, newFixture);
        }

        oldFixture.HomeTeamName = newFixture.HomeTeamName;
        oldFixture.HomeOrganisationLogo = newFixture.HomeOrganisationLogo;
        oldFixture.HomeScore = newFixture.HomeScore;

        oldFixture.AwayTeamName = newFixture.AwayTeamName;
        oldFixture.AwayOrganisationLogo = newFixture.AwayOrganisationLogo;
        oldFixture.AwayScore = newFixture.AwayScore;

        oldFixture.Date = newFixture.Date;
        oldFixture.Status = newFixture.Status;
        
        oldFixture.VenueName = newFixture.VenueName;
        oldFixture.Address = newFixture.Address;
        oldFixture.Longitude = newFixture.Longitude;
        oldFixture.Latitude = newFixture.Latitude;
    }

    protected override async Task InsertEntityAsync(Fixture newEntity)
    {
        await _fixtureRepository.AddAsync(newEntity);
    }

    protected override Task RemoveEntityAsync(Fixture oldEntity)
    {
        _fixtureRepository.Remove(oldEntity);

        return Task.CompletedTask;
    }

    protected override async Task OnMergeCompleteAsync()
    {
        await _fixtureRepository.SaveChangesAsync();
    }
}
