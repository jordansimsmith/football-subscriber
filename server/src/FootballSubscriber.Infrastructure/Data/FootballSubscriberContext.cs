using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace FootballSubscriber.Infrastructure.Data;

public class FootballSubscriberContext : DbContext
{
    public FootballSubscriberContext(DbContextOptions<FootballSubscriberContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}