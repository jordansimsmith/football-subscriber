using FootballSubscriber.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FootballSubscriber.Infrastructure;

public static class ServiceExtensions
{
    public static void AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FootballSubscriberContext>(options => { options.UseSqlServer(connectionString); });
    }

    public static void AddHangfireContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<HangfireContext>(options => { options.UseSqlServer(connectionString); });
    }
}