using System.Threading.Tasks;
using FootballSubscriber.Core.Models;

namespace FootballSubscriber.Core.Interfaces;

public interface IUserProfileService
{
    /// <summary>
    ///     Gets the user profile for the user id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<UserProfile> GetUserProfileAsync(string userId);
}