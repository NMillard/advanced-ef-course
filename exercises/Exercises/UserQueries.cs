using Domain;
using Microsoft.EntityFrameworkCore;
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
    public async Task QueryingUser()
    {
        List<User> e = context.Users
            .Include(u => u.UserSettings)
            .ThenInclude(u => u.Tier)
            .Include(u => u.Profiles)
            .ToList();
        
        testOutputHelper.WriteLine(e.Count.ToString());
    }

    [Fact]
    public void QueryingUserWithSettings()
    {
        List<User> e = context.Users
            .Include(u => u.Profiles)
            .ToList();
        
        testOutputHelper.WriteLine(e.Count.ToString());
    }
}