using Auth0.ManagementApi;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Infrastructure.Data;
using FootballSubscriber.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FootballSubscriber.Infrastructure;

public static class ServiceExtensions
{
    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FootballSubscriberContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }

    public static void AddHangfireContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<HangfireContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }

    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Competition>, Repository<Competition>>();
        services.AddScoped<IRepository<Fixture>, Repository<Fixture>>();
        services.AddScoped<IRepository<Subscription>, Repository<Subscription>>();
        services.AddScoped<IRepository<Team>, Repository<Team>>();

        services.AddScoped<IFixtureApiService, FixtureApiService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserProfileService, UserProfileService>();

        services.AddScoped<IManagementConnection, HttpClientManagementConnection>();
    }
}
