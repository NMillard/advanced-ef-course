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
        
    }
}

// TODO: Implement
/*
 * Add three entries to the UserTiers table.
 * id = 1 TierName = Free
 * id = 2 TierName = Basic
 * id = 3 TierName = Premium
 *
 * Hint: use builder.HasData()
 */
internal class UserTiersData : IEntityTypeConfiguration<UserTier>
{
    public void Configure(EntityTypeBuilder<UserTier> builder)
    {
        
    }
}