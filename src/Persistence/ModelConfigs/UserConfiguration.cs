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
internal class UserConfiguration : AuditableEntityConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        
        // Start wit this only
        builder.ToTable("Users", "Users");

        // Then tweak.
        builder.Property(u => u.Username).HasMaxLength(100).IsRequired();
        builder.HasOne(u => u.Settings) // EF tries to decide the parent table
            // It's good practice to be as explicit as possible when defining parent child relationships.
            .WithOne()
            .HasForeignKey(typeof(UserSettings), "UserId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        // Auto include relations to avoid partially hydrated objects.
        builder.Navigation(u => u.Settings)
            .UsePropertyAccessMode(PropertyAccessMode.Property)
            .AutoInclude();
    }
}