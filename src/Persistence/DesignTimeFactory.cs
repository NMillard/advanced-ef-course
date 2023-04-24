using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence;

/*
 * This relies on AppDbContext is created
 * 
 * It's unprofessional to commit sensitive data and secrets to source code.
 * For local development and build pipelines, use user-secrets.
 *
 * 1. run `dotnet user-secrets init`
 * 2. `dotnet user-secrets set ConnectionStrings:sql "Server=localhost,1499;Database=AdvancedORM;User ID=sa;Password=docker12*;Trusted_Connection=False;Persist Security Info=False;Encrypt=False"`
 *
 * Remember to include the appsettings.json when the project is build.
 *
 * <ItemGroup>
 *  <None Update="appsettings.json">
 *      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
 *  </None>
 * </ItemGroup>
 *
 * Now run `dotnet ef migrations add Initial`
 */
[UsedImplicitly]
internal class DesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public DesignTimeFactory() { }

    public DesignTimeFactory(string connectionString) => this.connectionString = connectionString;
    
    private readonly string? connectionString;

    public AppDbContext CreateDbContext(string[] args)
    {
        IConfiguration configurations = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<DesignTimeFactory>()
                .Build()
            ;

        string? s = connectionString ?? configurations.GetConnectionString("sql");
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder()
            .UseSqlServer(s, optionsBuilder =>
            {
                optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory");
            });

        return new AppDbContext(builder.Options);
    }
}