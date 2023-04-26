using Microsoft.Extensions.Configuration;
using Persistence;

namespace Exercises;

public class DatabaseFixture
{
    public DatabaseFixture()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .AddUserSecrets<DatabaseFixture>()
            .Build();

        string connectionString = configuration.GetConnectionString("sql") ?? throw new InvalidOperationException("Missing connection string");
        DbContext = new DesignTimeFactory(connectionString).CreateDbContext(null!);
    }

    internal AppDbContext DbContext { get; private set; }
}