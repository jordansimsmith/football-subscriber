using System.Collections.Generic;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Services;

public class TeamService : ITeamService
{
    private readonly IRepository<Team> _teamRepository;

    public TeamService(IRepository<Team> teamRepository)
    {
        _teamRepository = teamRepository;
    }

    public async Task<IEnumerable<Team>> GetTeamsAsync()
    {
        return await _teamRepository.FindAsync(t => true, t => t.ApiId);
    }
}