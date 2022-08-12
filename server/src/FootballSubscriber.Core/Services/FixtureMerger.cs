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
            && (oldFixture.Date != newFixture.Date || oldFixture.VenueId != newFixture.VenueId)
        )
        {
            // notify subscribers
            await _fixtureChangeNotificationService.NotifySubscribersAsync(oldFixture, newFixture);
        }

        oldFixture.CompetitionApiId = oldFixture.CompetitionApiId;
        oldFixture.Competition = oldFixture.Competition;

        oldFixture.HomeTeamApiId = newFixture.HomeTeamApiId;
        oldFixture.HomeTeamName = newFixture.HomeTeamName;
        oldFixture.HomeTeam = newFixture.HomeTeam;
        oldFixture.HomeOrganisationId = newFixture.HomeOrganisationId;
        oldFixture.HomeOrganisationLogo = newFixture.HomeOrganisationLogo;

        oldFixture.AwayTeamApiId = newFixture.HomeTeamApiId;
        oldFixture.AwayTeamName = newFixture.AwayTeamName;
        oldFixture.AwayTeam = newFixture.AwayTeam;
        oldFixture.AwayOrganisationId = newFixture.AwayOrganisationId;
        oldFixture.AwayOrganisationLogo = newFixture.AwayOrganisationLogo;

        oldFixture.Date = newFixture.Date.ToUniversalTime();
        oldFixture.VenueId = newFixture.VenueId;
        oldFixture.VenueName = newFixture.VenueName;
        oldFixture.Address = newFixture.Address;
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
