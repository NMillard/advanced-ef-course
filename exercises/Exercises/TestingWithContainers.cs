using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit.Abstractions;

namespace Exercises;

// https://dotnet.testcontainers.org/api/create_docker_container/

public class TestingWithContainers
{
    private readonly ITestOutputHelper testOutputHelper;

    private IContainer msContainer = new ContainerBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithEnvironment("MSSQL_SA_PASSWORD", "testcontainer12*")
        .WithPortBinding("1433", true)
        .Build();

    public TestingWithContainers(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task StartContainer()
    {
        await msContainer.StartAsync().ConfigureAwait(false);
        ushort port = msContainer.GetMappedPublicPort(1433);

        testOutputHelper.WriteLine(port.ToString());
    }
}