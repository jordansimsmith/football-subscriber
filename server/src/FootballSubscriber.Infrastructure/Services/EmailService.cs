using System.Threading.Tasks;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Models;
using FootballSubscriber.Infrastructure.Models;
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

        public async Task SendFixtureChangeEmailAsync(UserProfile user, FixtureChangeModel fixtureChange)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var sender = new EmailAddress(_configuration["SendGrid:SenderAddress"],
                _configuration["SendGrid:SenderName"]);
            var templateId = _configuration["SendGrid:FixtureChangeTemplateId"];

            var recipient = new EmailAddress(user.Email, user.Name);

            var templateData = new FixtureChangeTemplateData
            {
                ApplicationUrl = _configuration["ApplicationUrl"],
                AwayTeam = fixtureChange.AwayTeam,
                HomeTeam = fixtureChange.HomeTeam,
                Name = user.Name,
                NewAddress = fixtureChange.NewAddress,
                NewDate = fixtureChange.NewDateTime.ToLongDateString(),
                NewTime = fixtureChange.NewDateTime.ToShortTimeString(),
                NewVenue = fixtureChange.NewVenue,
                OldAddress = fixtureChange.OldAddress,
                OldDate = fixtureChange.OldDateTime.ToLongDateString(),
                OldTime = fixtureChange.OldDateTime.ToShortTimeString(),
                OldVenue = fixtureChange.OldVenue
            };

            var msg = new SendGridMessage
            {
                From = sender,
                Subject = $"Fixture Change: {fixtureChange.HomeTeam} vs {fixtureChange.AwayTeam}",
                TemplateId = templateId
            };
            msg.AddTo(recipient);
            msg.SetTemplateData(templateData);
            await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}