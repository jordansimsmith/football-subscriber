using FootballSubscriber.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSubscriber.Infrastructure.Data.Configurations
{
    public class FixtureConfiguration: IEntityTypeConfiguration<Fixture>
    {
        public void Configure(EntityTypeBuilder<Fixture> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.HomeTeamId).IsRequired();
            builder.Property(o => o.HomeTeamName).IsRequired();
            builder.Property(o => o.HomeOrganisationId).IsRequired();
            builder.Property(o => o.HomeOrganisationLogo);
            
            builder.Property(o => o.AwayTeamId).IsRequired();
            builder.Property(o => o.AwayTeamName).IsRequired();
            builder.Property(o => o.AwayOrganisationId).IsRequired();
            builder.Property(o => o.AwayOrganisationLogo);

            builder.Property(o => o.Date).IsRequired();
            builder.Property(o => o.VenueId).IsRequired();
            builder.Property(o => o.VenueName).IsRequired();
            builder.Property(o => o.Address);
        }
    }
}