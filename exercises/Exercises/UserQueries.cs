using Domain;
using Persistence;
using Xunit.Abstractions;

namespace Exercises;
/*
 * TODO: Before using this, remember to uncomment the code in DatabaseFixture.cs
 */
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
        // TODO: Uncomment this
        // var user = new User
        // {
        //     Username = "something",
        // };
        //
        // context.Users.Add(user);
        // context.SaveChanges();
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