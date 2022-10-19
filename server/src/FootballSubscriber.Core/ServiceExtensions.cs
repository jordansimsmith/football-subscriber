using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Mappers;
using FootballSubscriber.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FootballSubscriber.Core;

public static class ServiceExtensions
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IRefreshFixtureService, RefreshFixtureService>();
        services.AddScoped<ICompetitionService, CompetitionService>();
        services.AddScoped<IFixtureService, FixtureService>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IFixtureChangeNotificationService, FixtureChangeNotificationService>();

        services.AddScoped<IMerger<Competition>, CompetitionMerger>();
        services.AddScoped<IMerger<Fixture>, FixtureMerger>();

        services.AddAutoMapper(typeof(FixtureProfile));
    }
}
