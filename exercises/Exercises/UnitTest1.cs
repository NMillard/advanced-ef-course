using Domain;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Xunit.Abstractions;

namespace Exercises;

public class UnitTest1
{
    private readonly ITestOutputHelper testOutputHelper;
    
    private IContainer t = new ContainerBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithEnvironment("MSSQL_SA_PASSWORD", "testcontainer12*")
        .WithPortBinding("1433", true)
        .Build();
    
    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task QueryingUser()
    {
        await t.StartAsync().ConfigureAwait(false);
        ushort port = t.GetMappedPublicPort(1433);
        
        const string s = "";
        await using AppDbContext context = new DesignTimeFactory(s).CreateDbContext(null!);

        List<User> e = context.Users
            .Include(u => u.UserSettings)
            .ThenInclude(u => u.Tier)
            .Include(u => u.Profiles)
            .ToList();
        
        testOutputHelper.WriteLine(e.Count.ToString());
    }
}