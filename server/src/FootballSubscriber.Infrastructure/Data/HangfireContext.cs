using Microsoft.EntityFrameworkCore;

namespace FootballSubscriber.Infrastructure.Data;

public class HangfireContext : DbContext
{
    public HangfireContext(DbContextOptions<HangfireContext> options) : base(options) { }
}
