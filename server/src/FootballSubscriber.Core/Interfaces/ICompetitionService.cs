using System.Collections.Generic;
using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;

namespace FootballSubscriber.Core.Interfaces
{
    public interface ICompetitionService
    {
        /// <summary>
        ///     Gets current competitions
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Competition>> GetCompetitionsAsync();
    }
}