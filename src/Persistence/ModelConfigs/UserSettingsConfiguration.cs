using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ModelConfigs;

// TODO: Implement
/*
 * User Settings should be in the "Users" schema.
 */
internal class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder.ToTable("Settings", "Users");

        builder.Navigation(us => us.Tier).AutoInclude();
    }
}