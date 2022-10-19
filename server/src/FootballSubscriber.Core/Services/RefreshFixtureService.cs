using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Models;
using Microsoft.Extensions.Logging;

namespace FootballSubscriber.Core.Services;

public class RefreshFixtureService : IRefreshFixtureService
{
    private readonly IMerger<Competition> _competitionMerger;
    private readonly IRepository<Competition> _competitionRepository;
    private readonly IFixtureApiService _fixtureApiService;
    private readonly IMerger<Fixture> _fixtureMerger;
    private readonly IRepository<Fixture> _fixtureRepository;
    private readonly ILogger<RefreshFixtureService> _logger;
    private readonly IMapper _mapper;

    public RefreshFixtureService(
        IFixtureApiService fixtureApiService,
        IRepository<Fixture> fixtureRepository,
        IRepository<Competition> competitionRepository,
        IMerger<Fixture> fixtureMerger,
        IMerger<Competition> competitionMerger,
        IMapper mapper,
        ILogger<RefreshFixtureService> logger
    )
    {
        _fixtureApiService = fixtureApiService;
        _fixtureRepository = fixtureRepository;
        _competitionRepository = competitionRepository;
        _fixtureMerger = fixtureMerger;
        _competitionMerger = competitionMerger;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task RefreshFixturesAsync()
    {
        _logger.LogInformation("Refreshing fixtures");

        // get competitions from API
        var newCompetitions = (await _fixtureApiService.GetCompetitionsAsync()).ToList();
        await UpdateCompetitionsAsync(
            newCompetitions.Select(c => _mapper.Map<CompetitionModel, Competition>(c))
        );
        var localCompetitions = await _competitionRepository.FindAsync(c => true, f => f.ApiId);
        _logger.LogInformation("Retrieved competitions");

        // key: competitionApiId, value: List of fixtures
        var newFixtures = new ConcurrentDictionary<long, IList<Fixture>>();

        // get fixtures in parallel
        await Task.WhenAll(
            newCompetitions.Select(c => GetFixturesAsync(c, localCompetitions, newFixtures))
        );

        // set relationships between teams and fixtures
        foreach (var (competitionApiId, fixtures) in newFixtures)
        {
            await UpdateFixturesForCompetitionAsync(fixtures, competitionApiId);
        }
    }

    private async Task GetFixturesAsync(
        CompetitionModel competition,
        IEnumerable<Competition> localCompetitions,
        ConcurrentDictionary<long, IList<Fixture>> newFixtures
    )
    {
        // get organisations for the competition from the API
        var organisations = await _fixtureApiService.GetOrganisationsForCompetitionAsync(
            competition.Id
        );
        var organisationIds = organisations.Select(o => int.Parse(o.Id));

        // get fixtures for the competition from the API
        var fixturesResponse = await _fixtureApiService.GetFixturesForCompetitionAsync(
            competition.Id,
            organisationIds
        );
        var fixtures = fixturesResponse.Fixtures
            .Select(f =>
            {
                var fixture = _mapper.Map<FixtureModel, Fixture>(f);
                fixture.CompetitionApiId = competition.Id;
                fixture.Competition = localCompetitions.First(c => c.ApiId == competition.Id);

                // date from api is NZST but does not have any TZ info attached
                // we want to handle and store the dates in UTC on the server, and let the client convert back when retreiving
                var dateUnspecifiedKind = DateTime.SpecifyKind(f.Date, DateTimeKind.Unspecified);
                var nzTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(
                    "New Zealand Standard Time"
                );
                var utcDate = TimeZoneInfo.ConvertTimeToUtc(dateUnspecifiedKind, nzTimeZoneInfo);
                fixture.Date = utcDate;

                return fixture;
            })
            .ToList();

        newFixtures[competition.Id] = fixtures;
    }

    private async Task UpdateCompetitionsAsync(IEnumerable<Competition> competitions)
    {
        var oldCompetitions = (
            await _competitionRepository.FindAsync(c => true, f => f.ApiId)
        ).ToList();
        var newCompetitions = competitions.OrderBy(c => c.ApiId).ToList();

        await _competitionMerger.MergeAsync(oldCompetitions, newCompetitions);
    }

    private async Task UpdateFixturesForCompetitionAsync(
        IEnumerable<Fixture> fixtures,
        long competitionApiId
    )
    {
        var oldFixtures = (
            await _fixtureRepository.FindAsync(
                f => f.CompetitionApiId == competitionApiId,
                f => f.ApiId
            )
        ).ToList();
        var newFixtures = fixtures.OrderBy(f => f.ApiId).ToList();

        await _fixtureMerger.MergeAsync(oldFixtures, newFixtures);
    }
}
