using System.Threading.Tasks;

namespace FootballSubscriber.Core.Interfaces
{
    public interface IRefreshFixtureService
    {
        /// <summary>
        /// Refreshes the local cache of fixtures using the auckland football API
        /// </summary>
        /// <returns></returns>
        Task RefreshFixturesAsync();
    }
}