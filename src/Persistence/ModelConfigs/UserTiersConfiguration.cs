using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ModelConfigs;

// TODO: Implement
/*
 * The UserTiers table should exist in the Users schema.
 * TierName shouldn't allow more than 20 characters in the column and is required.
 */
internal class UserTiersConfiguration : IEntityTypeConfiguration<UserTier>
{
    public void Configure(EntityTypeBuilder<UserTier> builder)
    {
        builder.ToTable("Tiers", "Users");

        builder.Property(t => t.TierName).HasMaxLength(20).IsRequired();
    }
}

// TODO: Implement
/*
 * Add three entries to the UserTiers table.
 * id = 1 TierName = Free
 * id = 2 TierName = Basic
 * id = 3 TierName = Premium
 */
internal class UserTiersData : IEntityTypeConfiguration<UserTier>
{
    public void Configure(EntityTypeBuilder<UserTier> builder)
    {
        builder.HasData(new List<UserTier>
        {
            new(1, "Free"),
            new(2, "Basic"),
            new(3, "Premium"),
        });
    }
}