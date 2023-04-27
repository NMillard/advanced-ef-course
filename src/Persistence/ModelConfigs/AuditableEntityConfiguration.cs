using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ModelConfigs;

internal class AuditableEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class 
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        // Soft delete so we're not losing data
        builder.Property<DateTimeOffset?>("Deleted");
        builder.HasQueryFilter(u => EF.Property<DateTimeOffset?>(u, "Deleted") == null);

        // Typical additional properties that are irrelevant for the entity itself but important in terms of
        // tracability/auditability
        builder.Property<DateTimeOffset>("Created").HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();
        builder.Property<DateTimeOffset>("Updated").HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
    }
}