using FootballSubscriber.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSubscriber.Infrastructure.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.ApiId).IsUnique();

        builder.Property(o => o.Name).IsRequired();

        builder
            .HasMany(o => o.HomeFixtures)
            .WithOne(o => o.HomeTeam)
            .HasForeignKey(o => o.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasMany(o => o.AwayFixtures)
            .WithOne(o => o.AwayTeam)
            .HasForeignKey(o => o.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasMany(o => o.Subscriptions)
            .WithOne(o => o.Team)
            .HasForeignKey(o => o.TeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
