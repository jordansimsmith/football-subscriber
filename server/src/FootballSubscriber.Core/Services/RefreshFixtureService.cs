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
    private readonly IMerger<Team> _teamMerger;
    private readonly IRepository<Team> _teamRepository;

    public RefreshFixtureService(
        IFixtureApiService fixtureApiService,
        IRepository<Fixture> fixtureRepository,
        IRepository<Competition> competitionRepository,
        IMerger<Fixture> fixtureMerger,
        IMerger<Competition> competitionMerger,
        IMapper mapper,
        ILogger<RefreshFixtureService> logger,
        IRepository<Team> teamRepository,
        IMerger<Team> teamMerger
    )
    {
        _fixtureApiService = fixtureApiService;
        _fixtureRepository = fixtureRepository;
        _competitionRepository = competitionRepository;
        _fixtureMerger = fixtureMerger;
        _competitionMerger = competitionMerger;
        _mapper = mapper;
        _logger = logger;
        _teamRepository = teamRepository;
        _teamMerger = teamMerger;
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

        // key: ApiId, value: Team
        var newTeams = new ConcurrentDictionary<long, Team>();
        // key: competitionId, value: List of fixtures
        var newFixtures = new ConcurrentDictionary<int, IList<Fixture>>();

        // get fixtures in parallel
        await Task.WhenAll(
            newCompetitions.Select(
                c => GetFixturesAsync(c, localCompetitions, newTeams, newFixtures)
            )
        );

        // ensure database has up to date teams
        await UpdateTeamsAsync(newTeams.Values);
        var localTeams = (await _teamRepository.FindAsync(t => true, t => t.ApiId)).ToList();

        // set relationships between teams and fixtures
        foreach (var (competitionId, fixtures) in newFixtures)
        {
            foreach (var fixture in fixtures)
            {
                fixture.HomeTeam = localTeams.First(t => t.ApiId == fixture.HomeTeamApiId);
                fixture.AwayTeam = localTeams.First(t => t.ApiId == fixture.AwayTeamApiId);
            }

            await UpdateFixturesForCompetitionAsync(fixtures, competitionId);
        }
    }

    private async Task GetFixturesAsync(
        CompetitionModel competition,
        IEnumerable<Competition> localCompetitions,
        ConcurrentDictionary<long, Team> newTeams,
        ConcurrentDictionary<int, IList<Fixture>> newFixtures
    )
    {
        // get organisations for the competition from the API
        var competitionId = int.Parse(competition.Id);
        var organisations = await _fixtureApiService.GetOrganisationsForCompetitionAsync(
            competitionId
        );
        var organisationIds = organisations.Select(o => int.Parse(o.Id));

        // get fixtures for the competition from the API
        var fixturesResponse = await _fixtureApiService.GetFixturesForCompetitionAsync(
            competitionId,
            organisationIds
        );
        var fixtures = fixturesResponse.Fixtures
            .Select(f =>
            {
                var fixture = _mapper.Map<FixtureModel, Fixture>(f);
                fixture.CompetitionApiId = competitionId;
                fixture.Competition = localCompetitions.First(c => c.ApiId == competitionId);
                fixture.HomeTeamName ??= "Unknown";
                fixture.AwayTeamName ??= "Unknown";
                return fixture;
            })
            .ToList();

        // extract teams from the fixture information
        foreach (var fixture in fixtures)
        {
            newTeams[fixture.HomeTeamApiId] = new Team
            {
                ApiId = fixture.HomeTeamApiId,
                Name = fixture.HomeTeamName
            };

            newTeams[fixture.AwayTeamApiId] = new Team
            {
                ApiId = fixture.AwayTeamApiId,
                Name = fixture.AwayTeamName
            };
        }

        newFixtures[competitionId] = fixtures;
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
        int competitionId
    )
    {
        var oldFixtures = (
            await _fixtureRepository.FindAsync(
                f => f.CompetitionApiId == competitionId,
                f => f.ApiId
            )
        ).ToList();
        var newFixtures = fixtures.OrderBy(f => f.ApiId).ToList();

        await _fixtureMerger.MergeAsync(oldFixtures, newFixtures);
    }

    private async Task UpdateTeamsAsync(IEnumerable<Team> teams)
    {
        var oldTeams = (await _teamRepository.FindAsync(f => true, f => f.ApiId)).ToList();
        var newTeams = teams.OrderBy(f => f.ApiId).ToList();

        await _teamMerger.MergeAsync(oldTeams, newTeams);
    }
}
