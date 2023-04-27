using Domain;
using Persistence;
using Xunit.Abstractions;

namespace Exercises;

public class UserQueries : IClassFixture<DatabaseFixture>
{
    private readonly ITestOutputHelper testOutputHelper;
    private readonly AppDbContext context;

    public UserQueries(DatabaseFixture fixture, ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
        context = fixture.DbContext;
    }

    [Fact]
    public void SeedWithDefaultUser()
    {
        var user = new User
        {
            Username = "something",
        };

        context.Users.Add(user);
        context.SaveChanges();
    }

    [Fact]
    public async Task SelectAllUsers() { }

    [Fact]
    public async Task SelectUserByIdWithSettings() { }

    [Fact]
    public void SelectUserWithAutomaticEagerloadSettings() { }

    [Fact]
    public void Playground()
    {
        // Run your queries here.
    }
}