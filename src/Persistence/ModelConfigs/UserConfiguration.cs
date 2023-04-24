using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ModelConfigs;

/*
 * This class is dependent on DesignTimeFactory is created and configured.
 *
 * Start with minimal implementation only having the "ToTable" call. Then run a migration and inspect the result.
 * Come back to this class and tweak the table configuration as needed.
 */
internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Start wit this only
        builder.ToTable("Users", "Users");

        // Then tweak.
        builder.Property(u => u.Username).HasMaxLength(100).IsRequired();
        builder.HasOne(u => u.UserSettings) // EF tries to decide the parent table
            // It's good practice to be as explicit as possible when defining parent child relationships.
            .WithOne()
            // .HasPrincipalKey(typeof(User))
            .HasForeignKey(typeof(UserSettings), "UserId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasMany(u => u.Profiles)
            .WithMany()
            .UsingEntity<AuthorProfile>();
    }
}

internal class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder.ToTable("Settings", "Users");

        // Tweak
        builder.HasOne(us => us.Tier)
            .WithMany();
    }
}

internal class UserTiersConfiguration : IEntityTypeConfiguration<UserTier>
{
    public void Configure(EntityTypeBuilder<UserTier> builder)
    {
        builder.ToTable("Tiers", "Users");

        builder.Property(t => t.TierName).HasMaxLength(20).IsRequired();
    }
}

internal class UserTiersData : IEntityTypeConfiguration<UserTier>
{
    public void Configure(EntityTypeBuilder<UserTier> builder)
    {
        builder.HasData(new List<UserTier>
        {
            new(1, "Free"),
            new(2, "Free"),
            new(3, "Free"),
        });
    }
}

internal class AuthorProfilesConfiguration : IEntityTypeConfiguration<AuthorProfile>
{
    public void Configure(EntityTypeBuilder<AuthorProfile> builder)
    {
        builder.ToTable("AuthorProfiles", "Users");

        builder.Property<Guid>("UserId");
        builder.Property<Guid>("AuthorId");
        
        builder.HasKey("UserId", "AuthorId");
    }
}