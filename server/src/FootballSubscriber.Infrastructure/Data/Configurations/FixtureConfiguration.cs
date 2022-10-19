using FootballSubscriber.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSubscriber.Infrastructure.Data.Configurations;

public class FixtureConfiguration : IEntityTypeConfiguration<Fixture>
{
    public void Configure(EntityTypeBuilder<Fixture> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.ApiId).IsUnique();

        builder.Property(o => o.CompetitionApiId).IsRequired();

        builder.Property(o => o.HomeTeamName).IsRequired();
        builder.Property(o => o.HomeOrganisationLogo);
        builder.Property(o => o.HomeScore);

        builder.Property(o => o.AwayTeamName).IsRequired();
        builder.Property(o => o.AwayOrganisationLogo);
        builder.Property(o => o.AwayScore);

        builder.Property(o => o.Date).IsRequired();
        builder.Property(o => o.Status);

        builder.Property(o => o.VenueName).IsRequired();
        builder.Property(o => o.Address);
        builder.Property(o => o.Latitude);
        builder.Property(o => o.Longitude);

        builder
            .HasOne(o => o.Competition)
            .WithMany(o => o.Fixtures)
            .HasForeignKey(o => o.CompetitionId)
            .IsRequired();
    }
}
