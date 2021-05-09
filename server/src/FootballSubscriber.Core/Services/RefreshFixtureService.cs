using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Helpers;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Models;
using Microsoft.Extensions.Logging;

namespace FootballSubscriber.Core.Services
{
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
            IMerger<Team> teamMerger)
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

            var newCompetitions = (await _fixtureApiService.GetCompetitionsAsync()).ToList();
            await UpdateCompetitionsAsync(newCompetitions.Select(c => _mapper.Map<CompetitionModel, Competition>(c)));
            _logger.LogInformation("Retrieved competitions");

            var newTeams = new HashSet<Team>(new ApiEntityComparer());
            var newFixtures = new Dictionary<int, IList<Fixture>>();

            foreach (var competition in newCompetitions)
            {
                _logger.LogInformation($"Processing competition {competition.Name}");

                var competitionId = int.Parse(competition.Id);
                var organisations = await _fixtureApiService.GetOrganisationsForCompetitionAsync(competitionId);

                var organisationIds = organisations.Select(o => int.Parse(o.Id));

                var fixturesResponse =
                    await _fixtureApiService.GetFixturesForCompetitionAsync(competitionId, organisationIds);
                var localCompetitions = await _competitionRepository.FindAsync(c => true, f => f.ApiId);
                var fixtures = fixturesResponse.Fixtures.Select(f =>
                {
                    var fixture = _mapper.Map<FixtureModel, Fixture>(f);
                    fixture.CompetitionApiId = competitionId;
                    fixture.Competition = localCompetitions.First(c => c.ApiId == competitionId);
                    return fixture;
                }).ToList();

                foreach (var fixture in fixtures)
                {
                    newTeams.Add(new Team
                    {
                        ApiId = fixture.HomeTeamId,
                        Name = fixture.HomeTeamName
                    });
                    newTeams.Add(new Team
                    {
                        ApiId = fixture.AwayTeamId,
                        Name = fixture.AwayTeamName
                    });
                }

                newFixtures[competitionId] = fixtures;
            }

            await UpdateTeamsAsync(newTeams);
            var localTeams = (await _teamRepository.FindAsync(t => true, t => t.ApiId)).ToList();
            
            foreach (var (competitionId, fixtures) in newFixtures)
            {
                foreach (var fixture in fixtures)
                {
                    fixture.HomeTeam = localTeams.First(t => t.ApiId == fixture.HomeTeamApiId);
                    fixture.AwayTeam = localTeams.First(t => t.ApiId == fixture.HomeTeamApiId);
                }

                await UpdateFixturesForCompetitionAsync(fixtures, competitionId);
            }
        }

        private async Task UpdateCompetitionsAsync(IEnumerable<Competition> competitions)
        {
            var oldCompetitions = (await _competitionRepository.FindAsync(c => true, f => f.ApiId))
                .ToList();
            var newCompetitions = competitions.OrderBy(c => c.ApiId).ToList();

            await _competitionMerger.MergeAsync(oldCompetitions, newCompetitions);
        }

        private async Task UpdateFixturesForCompetitionAsync(IEnumerable<Fixture> fixtures, int competitionId)
        {
            var oldFixtures =
                (await _fixtureRepository.FindAsync(f => f.CompetitionApiId == competitionId, f => f.ApiId))
                .ToList();
            var newFixtures = fixtures.OrderBy(f => f.ApiId).ToList();

            await _fixtureMerger.MergeAsync(oldFixtures, newFixtures);
        }

        private async Task UpdateTeamsAsync(IEnumerable<Team> teams)
        {
            var oldTeams =
                (await _teamRepository.FindAsync(f => true, f => f.ApiId))
                .ToList();
            var newTeams = teams.OrderBy(f => f.ApiId).ToList();

            await _teamMerger.MergeAsync(oldTeams, newTeams);
        }
    }
}