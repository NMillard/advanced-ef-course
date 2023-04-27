using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ModelConfigs;

// TODO: Implement
/*
 * Articles should be in the "Articles" schema.
 * Title cannot be longer than 200 characters and is required.
 * Subtitle cannot be longer than 250 characters and is NOT required.
 */
public class ArticlesConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        
    }
}