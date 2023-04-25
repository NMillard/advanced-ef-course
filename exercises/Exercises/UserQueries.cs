using Domain;
using Persistence;
using Xunit.Abstractions;

namespace Exercises;

public class UserQueries : IClassFixture<DatabaseFixture>
{
    public UserQueries(DatabaseFixture fixture, ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        context = fixture.DbContext;
    }
    
    private readonly ITestOutputHelper testOutputHelper;
    private readonly AppDbContext context;

    [Fact]
    public async Task SimpleQuery()
    {
        List<User> e = context.Users.ToList();
    }

    
    [Fact]
    public void QueryingUserWithSettings()
    {
        // Update the user configuration 
    }
}