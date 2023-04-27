using Microsoft.Extensions.Configuration;
using Persistence;

namespace Exercises;

// TODO: Add an appsettings.test.json in the Exercises project with the following content:
/*
{
  "ConnectionStrings": {
    "sql": "the connection string"
  }
}
 */
public class DatabaseFixture
{
    public DatabaseFixture()
    {
        // TODO: Uncomment this
        // IConfiguration configuration = new ConfigurationBuilder()
        //     .AddJsonFile("appsettings.test.json")
        //     .AddUserSecrets<DatabaseFixture>()
        //     .Build();
        //
        // string connectionString = configuration.GetConnectionString("sql") ?? throw new InvalidOperationException("Missing connection string");
        // DbContext = new DesignTimeFactory(connectionString).CreateDbContext(null!);
    }

    internal AppDbContext DbContext { get; private set; }
}