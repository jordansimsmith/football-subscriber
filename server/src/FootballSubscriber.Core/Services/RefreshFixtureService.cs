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
        private readonly IFixtureApiService _fixtureApiService;
        private readonly IRepository<Fixture> _fixtureRepository;
        private readonly ILogger<RefreshFixtureService> _logger;
        private readonly IMapper _mapper;

        public RefreshFixtureService(
            IFixtureApiService fixtureApiService,
            IRepository<Fixture> fixtureRepository,
            IMapper mapper,
            ILogger<RefreshFixtureService> logger)
        {
            _fixtureApiService = fixtureApiService;
            _fixtureRepository = fixtureRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task RefreshFixturesAsync()
        {
            _logger.LogInformation("Refreshing fixtures");

            var competitions = await _fixtureApiService.GetCompetitionsAsync();
            _logger.LogInformation("Retrieved competitions");

            foreach (var competition in competitions)
            {
                _logger.LogInformation($"Processing competition {competition.Name}");

                var competitionId = int.Parse(competition.Id);
                var organisations = await _fixtureApiService.GetOrganisationsForCompetitionAsync(competitionId);

                var organisationIds = organisations.Select(o => int.Parse(o.Id));

                var fixturesResponse =
                    await _fixtureApiService.GetFixturesForCompetitionAsync(competitionId, organisationIds);
                var fixtures = fixturesResponse.Fixtures.Select(f =>
                {
                    var fixture = _mapper.Map<FixtureModel, Fixture>(f);
                    fixture.CompetitionId = competitionId;
                    return fixture;
                });

                await UpdateFixturesForCompetitionAsync(fixtures, competitionId);
            }
        }

        private async Task UpdateFixturesForCompetitionAsync(IEnumerable<Fixture> fixtures, int competitionId)
        {
            var oldFixtures = (await _fixtureRepository.FindAsync(f => f.CompetitionId == competitionId, f => f.ApiId))
                .ToList();
            var newFixtures = fixtures.OrderBy(f => f.ApiId).ToList();

            var i = 0; // old fixture counter
            var j = 0; // new fixture counter

            // while there are remaining new and old fixtures
            while (i < oldFixtures.Count && j < newFixtures.Count)
            {
                // exists in both old and new - UPDATE
                if (oldFixtures[i].ApiId == newFixtures[j].ApiId)
                {
                    // TODO: notify subscribers that fixtures have been updated
                    
                    oldFixtures[i].HomeTeamId = newFixtures[j].HomeTeamId;
                    oldFixtures[i].HomeTeamName = newFixtures[j].HomeTeamName;
                    oldFixtures[i].HomeOrganisationId = newFixtures[j].HomeOrganisationId;
                    oldFixtures[i].HomeOrganisationLogo = newFixtures[j].HomeOrganisationLogo;
                    oldFixtures[i].AwayTeamId = newFixtures[j].AwayTeamId;
                    oldFixtures[i].AwayTeamName = newFixtures[j].AwayTeamName;
                    oldFixtures[i].AwayOrganisationId = newFixtures[j].AwayOrganisationId;
                    oldFixtures[i].AwayOrganisationLogo = newFixtures[j].AwayOrganisationLogo;
                    oldFixtures[i].Date = newFixtures[j].Date;
                    oldFixtures[i].VenueId = newFixtures[j].VenueId;
                    oldFixtures[i].VenueName = newFixtures[j].VenueName;
                    oldFixtures[i].Address = newFixtures[j].Address;

                    i++;
                    j++;
                    continue;
                }

                // exists in new but not in old - INSERT
                if (oldFixtures[i].ApiId > newFixtures[j].ApiId)
                {
                    await _fixtureRepository.AddAsync(newFixtures[j]);

                    j++;
                    continue;
                }

                // exists in old but not in new - DELETE
                if (oldFixtures[i].ApiId < newFixtures[j].ApiId)
                {
                    _fixtureRepository.Remove(oldFixtures[i]);

                    i++;
                }
            }

            // while there are remaining old fixtures
            while (i < oldFixtures.Count)
            {
                _fixtureRepository.Remove(oldFixtures[i]);

                i++;
            }
            
            // while there are remaining new fixtures
            while (j < newFixtures.Count)
            {
                await _fixtureRepository.AddAsync(newFixtures[j]);

                j++;
            }

            await _fixtureRepository.SaveChangesAsync();
        }
    }
}