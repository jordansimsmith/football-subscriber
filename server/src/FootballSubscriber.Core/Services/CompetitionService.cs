using System.Collections.Generic;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Services;

public class CompetitionService : ICompetitionService
{
    private readonly IRepository<Competition> _competitionRepository;

    public CompetitionService(IRepository<Competition> competitionRepository)
    {
        _competitionRepository = competitionRepository;
    }

    public Task<IEnumerable<Competition>> GetCompetitionsAsync()
    {
        return _competitionRepository.FindAsync(c => true, c => c.ApiId);
    }
}