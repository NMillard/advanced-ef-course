using Persistence;
using Xunit.Abstractions;

namespace Exercises;

public class ArticlesQueries : IClassFixture<DatabaseFixture>
{

    private readonly ITestOutputHelper testOutputHelper;
    private readonly AppDbContext context;

    public ArticlesQueries(DatabaseFixture fixture, ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        context = fixture.DbContext;
    }
    

    [Fact]
    public void Playground()
    {
        // Run your queries here.
    }
}