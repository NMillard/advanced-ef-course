using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

/**
 * You'd never want to expose your db context class. It's infrastructure and concrete implementations
 * shouldn't be relied on by others.
 */
internal class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    
    /*
     * A common mistake is to include every entity and value type in the AppDbContext.
     * Only add aggregate roots here.
     */
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableDetailedErrors();

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly); // Support open/closed principal
    }
}