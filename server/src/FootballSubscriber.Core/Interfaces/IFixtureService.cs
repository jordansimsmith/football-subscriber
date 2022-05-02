using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;

namespace FootballSubscriber.Core.Interfaces;

public interface IFixtureService
{


    /// <summary>
    ///     Gets current fixtures for a competition and date range
    /// </summary>
    /// <param name="competitionId"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    Task<IEnumerable<Fixture>> GetFixturesAsync(int competitionId, DateTime fromDate, DateTime toDate);
}