---
title: 4 Advanced Functionality and Concepts
sidebar_position: 5
---

This section of the course explores some of the often-used EF Core advanced features.

## Auditable Entities

```csharp title="./src/Persistence/ModelConfigs"
namespace Persistence.ModelConfigs;

internal class AuditableEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class 
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        // Soft delete so we're not losing data
        builder.Property<DateTimeOffset?>("Deleted");
        builder.HasQueryFilter(u => EF.Property<DateTimeOffset?>(u, "Deleted") == null);

        // Typical additional properties that are irrelevant for the entity itself but important in terms of
        // tracability/auditability
        builder.Property<DateTimeOffset>("Created").HasDefaultValueSql("getutcdate()").ValueGeneratedOnAdd();
        builder.Property<DateTimeOffset>("Updated").HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
    }
}

// Let UserConfiguration inhert the class
internal class UserConfiguration
    //highlight-next-line
    : AuditableEntityConfiguration<User>, IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //highlight-next-line
        base.Configure(builder);

        ...
    }
}
```

## Audit Entity Configuration Base Implementation
Move Created, Updated, and Deleted into base type.

## Converters
EF Core supports custom converters that allow you to map custom structures and properties to database columns. Custom converters are useful when you need to store complex data types in a database column, such as enums or custom objects. By defining a custom converter, you can specify how EF Core should convert the data type to and from the database column. This allows you to work with complex data types in your application code, while still storing them in a structured format in the database.

Update the `Article` model with a list property that holds `CategoryTag`.

```csharp title="./src/Domain/Article.cs"
public class Article
{
    //highlight-start
    private readonly List<CategoryTag> tags;
    
    public Article()
    {
        tags = new List<CategoryTag>();
    }
    //highlight-end

    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? SubTitle { get; set; }
    
    public string? Content { get; set; }
    
    /// <summary>
    /// The image that is commonly displayed at the top of an article.
    /// </summary>
    public byte[]? PictureLead { get; set; }

    //highlight-next-line
    public IEnumerable<CategoryTag> Tags => tags.AsReadOnly();
}
```

Attempting to run `dotnet ef migrations add ArticlesTags` fails. EF Core expects `CategoryTag` to have its own `id` property, and since the tag is a value object, that is, an object without its own identity and cannot live without being contained within an other, we wouldn't want it to have an `id`.

For just a second, let's make EF Core happy and implement its suggested change to `CategoryTag`.

```csharp title="./src/Domain/Article.cs"
...
public record CategoryTag(
    //highlight-next-line
    Guid id,
    string TagName
);
...
```

Then run `dotnet ef migrations add ArticlesTags`.
This migrations now contains the creation of an entire table with a foreign key to `Article`.

```csharp
...
migrationBuilder.CreateTable(
    name: "CategoryTag",
    columns: table => new
    {
        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
        TagName = table.Column<string>(type: "nvarchar(max)", nullable: false),
        ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
    }
...
```

While this works perfectly fine, it might be a overkill and potentially a performance killer due to the additional database roundtrip to a table that may contains millions of entries.

#### Creating a converter
```csharp title="./src/Persistence/ModelConfigs/ArticlesConfiguration.cs"
public class ArticlesConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        ...
        
        //highlight-start
        builder.Property(a => a.Tags).HasConversion(
            tags => string.Join(",", tags),
            dbValue => dbValue.Split(",", StringSplitOptions.None).Select(tag => new CategoryTag(tag))
        );
        //highlight-end
    }
}
```


## Object-relational Impedance Mismatch
The challenges that arise from attempting to map classes from object-oriented languages to a relational, column-based structure such as RDBMS.


## Many-To-Many
Implicit join infrastructure type, move tags out of articles and into join table.

## Proper Encapsulation
You've likely noticed how most models have public setters. This is typically a bad practice because domain classes don't protect their invariants, and you or other developers can create non-valid objects, e.g. "user without a username".

## Materialized Views
Map a synthetic entity to a database view.

## Pagination
1. Offset-based
2. Cursor-based

## Rolling Back a Migration