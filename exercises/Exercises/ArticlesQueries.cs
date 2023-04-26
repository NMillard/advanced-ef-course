using Persistence;
using Xunit.Abstractions;

namespace Exercises;

public class ArticlesQueries : IClassFixture<DatabaseFixture>
{
    private readonly ITestOutputHelper testOutputHelper;

    public ArticlesQueries(DatabaseFixture fixture, ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        context = fixture.DbContext;
    }
    
    private readonly AppDbContext context;

    [Fact]
    public void DemoQueries()
    {
        // Run your queries here.
    }
}