---
title: 3 Querying Data
sidebar_position: 5
---

So far we've only set up the database with tables.

Let's see how you can query data. Open the `./exercises/Exercises/UserQueries.cs` test class, and start experimenting with different queries.

## Including Nested Objects and Collection Properties
One of the benefits of using Entity Framework Core is its ability to easily handle nested objects and collections. In EF Core, you can use the Include() method to eagerly load related data, which can improve performance and reduce the number of database roundtrips.


### Auto-including
It's a good practice to always keep instances in their fully hydrated state. That is, if you have a `User` with the property `Settings`, you'd expect the property to reference an object. But, when using an OR/M, that's not always the case. You may retrieve only partially hydrated objects because you forgot an `Include()`.

To avoid this issue, you can auto-include navigation properties by updating the entity configuration. This is especially helpful with large object graphs. In the real world, it's common to see aggregate roots with more than 15 navigation properties.


#### Auto-include `Settings` when `User` is queried
To make use of the Include() method, you can configure your entities using the `IEntityTypeConfiguration<T>` interface. In particular, you can use the Navigation method to specify the navigation property on the entity and enable auto inclusion. Auto inclusion will automatically include the related data when querying the entity, which can be especially helpful for complex object graphs with many related entities.


```csharp title="./src/Persistence/ModelConfigs/UserConfiguration.cs"
internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ...

        //highlight-start
        // Auto include relations to avoid partially hydrated objects.
        builder.Navigation(u => u.Settings)
            .UsePropertyAccessMode(PropertyAccessMode.Property)
            .AutoInclude();
        //highlight-end

        ...
    }
}
```
There's no migration needed since it is purely a behavioural change to EF Core when querying data.

## Soft deleting
Sometimes you want to delete rows (entities) without actually losing the data.

:::note
This concept is a must-know for any data- and software professional.
:::

Soft delete is a database design pattern that involves marking a row or record as "deleted" instead of actually deleting it from the database. This is in contrast to a hard delete, where the record is permanently removed from the database.

Soft delete is useful for a number of reasons. First, it provides a way to keep a record of deleted data, which can be important for audit trails and compliance purposes. By keeping a record of deleted data, you can track when and why it was deleted, which can be useful for debugging and data analysis.

Second, soft delete can simplify data recovery in the event of accidental data loss or deletion. Because soft deleted data is not actually removed from the database, it can be easily recovered if needed.

Finally, soft delete can be a useful way to implement business rules around data deletion. For example, you might want to prevent users from deleting certain types of data, or require that data be reviewed and approved before it can be deleted. By implementing soft delete, you can enforce these rules while still providing a way to remove data from the active dataset.

In Entity Framework Core, soft delete can be implemented using a variety of techniques, such as adding a "Deleted" flag column to the entity or using a separate "DeletedItems" table to store soft deleted data. By using soft delete in your EF Core application, you can provide additional safeguards and functionality around data deletion, and ensure that you have a complete record of all changes to your data.

The "Deleted" flag should ideally be a date time offset instead of a boolean (bit), to provide that additional contextual information about _when_ the action was made.

#### Adding soft delete to `User`
```csharp title="./src/Persistence/ModelConfig/UserConfiguration.cs"
internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ...

        //highlight-start
        builder.Property<DateTimeOffset?>("Deleted");
        builder.HasQueryFilter(u => EF.Property<DateTimeOffset?>(u, "Deleted") == null);
        //highlight-end

        ...
    }
}
```

You're introduced to three new concepts in EF Core here.
1. Shadow properties: we're adding a property "Deleted" that doesn't exist on the CLR type (`User`). This requires a new migration.
2. Query filters: a way to apply global filters onto an entity. These can also be disabled.
3. Querying on shadow properties: by using `EF.Property()` you're allowed access to shadow properties. Whatever you select here isn't evaluated at design or build time, and may throw at run-time.