using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Services;

public class TeamService : ITeamService
{
    private readonly IRepository<Fixture> _fixtureRepository;

    public TeamService(IRepository<Fixture> fixtureRepository)
    {
        _fixtureRepository = fixtureRepository;
    }

    public async Task<IEnumerable<string>> GetTeamsAsync()
    {
        return (await _fixtureRepository.FindAsync(f => true, f => f.Id))
            .SelectMany(f => new[] { f.HomeTeamName, f.AwayTeamName })
            .Distinct()
            .ToArray();
    }
}
