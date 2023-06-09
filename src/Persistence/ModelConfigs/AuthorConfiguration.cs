using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ModelConfigs;

// TODO: Implement
/*
 * The Authors table should be in "Authors" schema.
 * The PenName should allow up to 100 characters and is required.
 */
public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors", "Authors");
        
        builder.HasKey(a => a.Id);
        builder.Property(a => a.PenName).HasMaxLength(100).IsRequired();
    }
}