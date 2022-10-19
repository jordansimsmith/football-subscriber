using System.Collections.Generic;
using System.Threading.Tasks;
using FootballSubscriber.Core.Models;

namespace FootballSubscriber.Core.Interfaces;

public interface ITeamService
{
    /// <summary>
    ///     Gets the list of teams
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TeamModel>> GetTeamsAsync();
}
