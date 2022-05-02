using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;

namespace FootballSubscriber.Core.Interfaces;

public interface IFixtureChangeNotificationService
{
    /// <summary>
    ///     Notifies the subscribers of a team that their fixtures have changed
    /// </summary>
    /// <param name="oldFixture"></param>
    /// <param name="newFixture"></param>
    /// <returns></returns>
    Task NotifySubscribersAsync(Fixture oldFixture, Fixture newFixture);
}