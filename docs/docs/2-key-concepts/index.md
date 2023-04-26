---
title: 2 Key concepts
sidebar_position: 4
---

This section covers the key concepts in EF Core that are essential for building robust, secure, and scalable applications.

In this section, you'll learn about the `DbContext`, `IEntityTypeConfiguration<T>`, converts, constraints, and migrations, as well as getting experience with a code-first approach. With a strong understanding of these key concepts, you'll be able to build efficient and maintainable applications using EntityFramework Core.

### Infrastructure Concerns
EntityFramework Core is an infrastructure oriented library, as it provides a low-level abstraction for interacting with a database. It is not part of the business logic or presentation layer of an application, but rather is responsible for managing the connection to the database and performing database operations. This separation of concerns is an essential aspect of building maintainable and scalable applications.

By separating infrastructure concerns like DbContext from the business logic and presentation layers, developers can ensure that their application is modular and easy to maintain. This allows developers to focus on the business logic of the application, without having to worry about the details of how data is stored and retrieved from the database. It also makes it easier to change the underlying database technology, without having to modify the business logic or presentation layer of the application.

We'll walk through examples of how to structure your application to ensure that infrastructure concerns are separated from business logic and presentation concerns.

First, take a second to scan the included packages in `./src/Persistence/Persistence.csproj`.

```xml
<ItemGroup>
        <InternalsVisibleTo Include="Exercises" />
    </ItemGroup>

    <ItemGroup>
        <!--adding EntityFramework Core with SQL server provider-->
        <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0-preview.3.23174.2" />
        <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0-preview.3.23174.2">
            <PrivateAssets>all</PrivateAssets>
            <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
        </PackageReference>
        <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0-preview.3.23174.2" />

        <!--to read appsettings.json and user secrets-->
        <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0-preview.3.23174.8" />
        <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.0-preview.3.23174.8" />
        <PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="8.0.0-preview.3.23174.8" />
    </ItemGroup>

    <ItemGroup>
        <ProjectReference Include="..\Domain\Domain.csproj" />
    </ItemGroup>

    <ItemGroup>
        <None Update="appsettings.json">
            <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
        </None>
    </ItemGroup>
```
We're using the version 8 preview since it has some neat updates that you can read [more about here](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/plan).

## The `DbContext`
The DbContext class is fundamental to how EntityFramework Core works as it serves as the primary entry point for accessing a database.

It provides a set of APIs that enable you to interact with the database and entities, including querying, saving, and updating data. The DbContext also handles database connection management, transaction management, and change tracking, allowing you to focus on writing code that interacts with the entities instead of managing database connections and transactions.

Setting up the `DbContext` is generally quite effortless, and only involves a few code changes.  

```csharp title="./src/Persistence/AppDbContext.cs"
internal class AppDbContext : DbContext
{
    // highlight-start
    public AppDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<User> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableDetailedErrors();
    }
    // highlight-end
}
```

Now, we have an access point to a table in a database - but, we still haven't actually created a script that'll generate the `Users` table.


## Generating Migrations
One of the powerful features of EF Core is its ability to generate migrations based on changes to the classes in your code. Migrations are the way that EF Core handles changes to the database schema over time. Instead of having to manually create and manage database scripts, you can simply make changes to your classes and let EF Core generate the necessary migration code.

When generating migrations with EF Core, it's important to always manually inspect the migration. If the migration seems okay in C#, then generate an idempotent SQL script. This is a best practice and always adviced when working on client projects.

You'll sometimes see teams run migrations as part of the application startup process. In .NET, this is an unprofessional approach to managing database changes through code. We'll get back to why that is, later.

1. Before creating a migration, go checkout the `User` class in `./src/Domain/User.cs`, and think about how you'd expect to store this in a relational database. The file contains 3 models.
1. Run `dotnet ef migrations add Initial` from within the `./src/Persistence` project


:::info

This should generate an error complaining about EF Core not knowing how to instantiate the `AppDbContext` and its options.
**We'll fix this in a second.**

:::


## Implementing a Design-Time Factory
When working with EF Core, if you only have a DbContext class and no startup project, attempting to run dotnet ef migrations add or dotnet ef database update commands will fail due to EF Core not knowing how to instantiate the DbContext class.

One way to solve this problem is by creating a factory class that implements the `IDesignTimeDbContextFactory<T>` interface. This interface allows you to create a DbContext instance at design time, which is needed for migrations and database updates.

To implement this solution, you need to create a new class that implements `IDesignTimeDbContextFactory<T>` and provides the necessary configuration for creating an instance of the DbContext. In the CreateDbContext method, you can configure the DbContext by passing in the appropriate options, such as the connection string and any other necessary configuration.

Once you have implemented the `IDesignTimeDbContextFactory<T>` interface, you can use the dotnet ef commands to create and apply migrations to your database, even without a startup project. The EF Core tooling will use the factory class to create an instance of the DbContext, and then use that instance to generate the migration code or apply updates to the database.

Using `IDesignTimeDbContextFactory<T>` allows you to separate the concerns of your application's runtime and design-time configurations, and provides a clean and scalable way to configure your DbContext.

```csharp
[UsedImplicitly]
internal class DesignTimeFactory 
// highlight-start
: IDesignTimeDbContextFactory<AppDbContext>
{
    public DesignTimeFactory()
    {
        IConfiguration configurations = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<DesignTimeFactory>()
                .Build();
        
        connectionString = configurations.GetConnectionString("Sql")  ?? string.Empty;
    }

    public DesignTimeFactory(string connectionString) : this() 
        => this.connectionString = connectionString;
    
    private readonly string? connectionString;

    public AppDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder builder = new DbContextOptionsBuilder()
            .UseSqlServer(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory");
            });

        return new AppDbContext(builder.Options);
    }
// highlight-end
}
```

Notice how the default constructor instantiates a `ConfigurationBuilder`and adds json file and user secrets to its configuration providers.  

Adding the user secrets is a good practice during local develop to ensure you're not committing a connection string to git, which may hold sensitive information.

The `Persistence.csproj` already contains a `<UserSecretsId>GUID</UserSecretsId>` property. If that wasn't the case, you'd have to run `dotnet user-secrets init`.


:::info
Set the secret ConnectionStrings:Sql by running:  
`dotnet user-secrets set ConnectionStrings:Sql "Server=localhost,1499;Database=AdvancedORM;User ID=sa;Password=docker12*;Trusted_Connection=False;Persist Security Info=False;Encrypt=False"`
:::

With the design-time factory implemented, we're now able to run `dotnet ef` commands without a startup project.

Remember, we're trying to create a migration for the following models:

```csharp
public class User
{
    public User()
    {
        Id = Guid.NewGuid();
        profiles = new List<AuthorProfile>();
    }

    private readonly List<AuthorProfile> profiles;

    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public UserSettings Settings { get; private set; }

    public IEnumerable<AuthorProfile> Profiles => profiles.AsReadOnly();
}

public class UserSettings
{
    public UserSettings()
    {
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; private set; }
    public UserTier Tier { get; private set; }
}

public record UserTier(int Id, string TierName);

```

1. Run the migrations add command again: `dotnet ef migrations add Initial`
2. Take some time to go inspect the output in `./src/Persistence/Migrations/<timestamp>_Initial.cs`


## Manual Inspection
Interestingly, despite only having added a property `DbSet<User> Users` to the `AppDbContext`, you'll see, the migration has created quite a few tables, including the tables `UserTier`, `UserSettings`, `Users`, `Author` and `Articles`.

EF Core walks the object graph and attempts to create tables for each entity it encounters.

By code insepction, it's evident that we can't trust EF Core to always make the best judgement calls. 
```csharp
migrationBuilder.CreateTable(
    name: "Users",
    columns: table => new
    {
        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
        // highlight-next-line
        Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
        // highlight-next-line
        UserSettingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Users", x => x.Id);
        table.ForeignKey(
            name: "FK_Users_UserSettings_UserSettingsId",
            column: x => x.UserSettingsId,
            principalTable: "UserSettings",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });
migrationBuilder.CreateTable(
    name: "Author",
    columns: table => new
    {
        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
        PenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
        // highlight-next-line
        UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Author", x => x.Id);
        table.ForeignKey(
            name: "FK_Author_Users_UserId",
            column: x => x.UserId,
            principalTable: "Users",
            principalColumn: "Id");
    });
```

There're plenty of things to address here, but three observations are particularly interesting:
1. EF picked the wrong parent table for the one-to-one relation between `User` and `UserSettings`.
2. We need a many-to-many relationship between `User` and `Author` by using a "join table/entity" `AuthorProfile`, but, EF Core didn't notice the `List profiles` in the `User` model.
3. `NVARCHAR` columns defaults to `max` which is a performance killer.

:::note
Remove the generated migrations simply by deleting the files.
:::

## Configuring Entities
By configuring your entities through `IEntityTypeConfiguration<T>` implementations, you can make fine-tuned adjustments to columns and provide EF Core with enough information to generate correct joins between tables.

Let's start by opening up the `./src/Persistence/ModelConfigs/UserConfiguration.cs`. The file contains a few classes without any implementation.

Start off by implementing the `UserConfiguration` and `AuthorProfilesConfiguration`.

```csharp title="src/Persistence/ModelConfigs/UserConfiguration.cs"
internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    // highlight-start
    builder.ToTable("Users", "Users");

    builder.Property(u => u.Username).HasMaxLength(100).IsRequired();

    builder.HasOne(u => u.Settings) 
            .WithOne()
            .HasForeignKey(typeof(UserSettings), "UserId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    // highlight-end
}
```

```csharp title="src/Persistence/ModelConfigs/AuthorProfilesConfiguration.cs"
internal class AuthorProfilesConfiguration  : IEntityTypeConfiguration<AuthorProfile>
{
    public void Configure(EntityTypeBuilder<AuthorProfile> builder)
    {
        // highlight-start
        builder.ToTable("AuthorProfiles", "Users");

        builder.Property<Guid>("UserId");
        builder.Property<Guid>("AuthorId");

        builder.HasOne(a => a.User);
        builder.HasOne(a => a.Author);
        builder.Property(a => a.IsAdministrator).IsRequired();
        
        builder.HasKey("UserId", "AuthorId");
        // highlight-end
    }
}
```

For this to take effect, you'll need to let `AppDbContext` know that it must scan the assembly for types that implements the `IEntityTypeConfiguration<T>` interface.

```csharp title="./src/Persistence/AppDbContext.cs"
internal class AppDbContext : DbContext
{
...
    // highlight-start
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly); // Support open/closed principal
    }
    // highlight-end
...
}
```


#### Run migration again
Then run `dotnet ef migrations add Initial`.

This time, the `Users`, `UserSetting` and `AuthorProfile` tables look much nicer.

```csharp
migrationBuilder.CreateTable(
    name: "Users",
    schema: "Users",
    columns: table => new
    {
        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
        // highlight-next-line
        Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Users", x => x.Id);
    });

migrationBuilder.CreateTable(
    name: "UserSettings",
    columns: table => new
    {
        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
        TierId = table.Column<int>(type: "int", nullable: false),
        // highlight-next-line
        UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_UserSettings", x => x.Id);
        table.ForeignKey(
            name: "FK_UserSettings_UserTier_TierId",
            column: x => x.TierId,
            principalTable: "UserTier",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        table.ForeignKey(
            name: "FK_UserSettings_Users_UserId",
            column: x => x.UserId,
            principalSchema: "Users",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });


migrationBuilder.CreateTable(
    name: "AuthorProfiles",
    schema: "Users",
    columns: table => new
    {
        // highlight-next-line
        UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
        // highlight-next-line
        AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
        IsAdministrator = table.Column<bool>(type: "bit", nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_AuthorProfiles", x => new { x.UserId, x.AuthorId });
        table.ForeignKey(
            name: "FK_AuthorProfiles_Author_AuthorId",
            column: x => x.AuthorId,
            principalTable: "Author",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        table.ForeignKey(
            name: "FK_AuthorProfiles_Users_UserId",
            column: x => x.UserId,
            principalSchema: "Users",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });
```

It's still not perfect, but definitely miles better than before.

With a somewhat okay solution, let's generate a database script from our migration.
:::info
Run `dotnet ef migrations script -i -o scripts/db.sql`  
Remember, `pwd` should be the Persistence project.
:::

Find the newly generated SQL script in `./src/Persistence/scripts/db.sql` and apply it to your database.

## Exercises
Implement the remaining entity type configuration classes. They're all located in the `Persistence.ModelConfigs` namespace.

## Next up
In the next section, we'll get experiri (that is experience through experimentation) with querying data.