using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ModelConfigs;

public class ArticlesConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles", "Articles");

        builder.Property(a => a.Title).HasMaxLength(200).IsRequired();
        builder.Property(a => a.SubTitle).HasMaxLength(250);
        builder.Property(a => a.Content);

        builder.Property(a => a.Tags).HasConversion(
            tags => string.Join(",", tags),
            s => s.Split(",", StringSplitOptions.None).Select(s1 => new CategoryTag(s1))
        );
    }
}