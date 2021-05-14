using System.Threading.Tasks;

namespace FootballSubscriber.Core.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        ///     Sends an email to the recipient with the supplied subject and content
        /// </summary>
        /// <param name="recipientAddress"></param>
        /// <param name="recipientName"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task SendEmailAsync(string recipientAddress, string recipientName, string subject, string content);
    }
}