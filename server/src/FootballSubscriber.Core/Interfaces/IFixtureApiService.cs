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

        /// <summary>
        /// Gets the list of organisations involved in a competition from the auckland football API
        /// </summary>
        /// <param name="competitionId"></param>
        /// <returns></returns>
        Task<IEnumerable<OrganisationModel>> GetOrganisationsForCompetition(int competitionId);

        /// <summary>
        /// Gets the list of fixtures for a competition from the auckland football API
        /// </summary>
        /// <param name="competitionId"></param>
        /// <param name="organisationIds"></param>
        /// <returns></returns>
        Task<GetFixturesResponseModel> GetFixturesForCompetition(int competitionId, IEnumerable<int> organisationIds);
    }
}