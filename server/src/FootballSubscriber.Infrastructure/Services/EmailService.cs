using System.Threading.Tasks;
using FootballSubscriber.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FootballSubscriber.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string recipientAddress, string recipientName, string subject, string content)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var sender = new EmailAddress(_configuration["SendGrid:SenderAddress"],
                _configuration["SendGrid:SenderName"]);
            var recipient = new EmailAddress(recipientAddress, recipientName);

            var msg = new SendGridMessage
            {
                From = sender,
                Subject = subject,
                PlainTextContent = content
            };
            msg.AddTo(recipient);
            await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}