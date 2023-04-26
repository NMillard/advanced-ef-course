using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ModelConfigs;

internal class AuthorProfilesConfiguration  : IEntityTypeConfiguration<AuthorProfile>
{
    public void Configure(EntityTypeBuilder<AuthorProfile> builder)
    {
        builder.ToTable("AuthorProfiles", "Users");

        builder.Property<Guid>("UserId");
        builder.Property<Guid>("AuthorId");

        builder.HasOne(a => a.User);
        builder.HasOne(a => a.Author);
        builder.Property(a => a.IsAdministrator).IsRequired();
        
        builder.HasKey("UserId", "AuthorId");
    }
}