using System.Collections.Generic;
using System.Threading.Tasks;
using FootballSubscriber.Core.Models;

namespace FootballSubscriber.Core.Interfaces
{
    public interface IFixtureApiService
    {
        /// <summary>
        /// Gets the list of competitions from the auckland football API
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CompetitionModel>> GetCompetitionsAsync();
    }
}