using FootballSubscriber.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSubscriber.Infrastructure.Data.Configurations;

public class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
{
    public void Configure(EntityTypeBuilder<Competition> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.ApiId).IsUnique();

        builder.Property(o => o.Name).IsRequired();

        builder.HasMany(o => o.Fixtures).WithOne(o => o.Competition);
    }
}
