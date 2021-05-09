using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FootballSubscriber.Core.Entities;
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

        public RefreshFixtureService(
            IFixtureApiService fixtureApiService,
            IRepository<Fixture> fixtureRepository,
            IRepository<Competition> competitionRepository,
            IMerger<Fixture> fixtureMerger,
            IMerger<Competition> competitionMerger,
            IMapper mapper,
            ILogger<RefreshFixtureService> logger)
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

            var competitions = (await _fixtureApiService.GetCompetitionsAsync()).ToList();
            await UpdateCompetitionsAsync(competitions.Select(c => _mapper.Map<CompetitionModel, Competition>(c)));
            _logger.LogInformation("Retrieved competitions");

            foreach (var competition in competitions)
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
                });

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
    }
}