using FootballSubscriber.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FootballSubscriber.Infrastructure.Data.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => new { o.TeamName, o.UserId }).IsUnique();

        builder.Property(o => o.UserId).IsRequired();
    }
}