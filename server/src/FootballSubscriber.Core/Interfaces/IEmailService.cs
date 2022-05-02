using System.Threading.Tasks;
using FootballSubscriber.Core.Models;

namespace FootballSubscriber.Core.Interfaces;

public interface IEmailService
{
    /// <summary>
    ///     Notifies the user that a fixture they have subscribed to has changed, via email
    /// </summary>
    /// <param name="user"></param>
    /// <param name="fixtureChange"></param>
    /// <returns></returns>
    public Task SendFixtureChangeEmailAsync(UserProfile user, FixtureChangeModel fixtureChange);
}