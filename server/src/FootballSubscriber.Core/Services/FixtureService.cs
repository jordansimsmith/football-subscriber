using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Services;

public class FixtureService : IFixtureService
{
    private readonly IRepository<Fixture> _fixtureRepository;

    public FixtureService(IRepository<Fixture> fixtureRepository)
    {
        _fixtureRepository = fixtureRepository;
    }

    public Task<IEnumerable<Fixture>> GetFixturesAsync(int competitionId, DateTime fromDate, DateTime toDate)
    {
        return _fixtureRepository.FindAsync(
            c => c.CompetitionId == competitionId && c.Date >= fromDate && c.Date <= toDate,
            c => c.ApiId);
    }
}