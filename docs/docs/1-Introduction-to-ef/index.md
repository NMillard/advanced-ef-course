---
title: 1 Introduction to OR/M and EF
sidebar_position: 3
---

Object-Relational Mappers (OR/Ms) are software tools that enable developers to interact with relational databases using an object-oriented paradigm. EntityFramework Core (EF Core) is one of the most popular OR/Ms in the .NET ecosystem. It allows developers to map database tables to classes, and provides an abstraction layer between the application and the database.

EF Core is a powerful and advanced tool that enables senior developers to build robust and scalable applications. However, it can also be easily misused by junior developers who lack a deep understanding of its concepts and capabilities. Junior developers tend to rely too heavily on the apparent ease by which you can create and maintain a database, without ever being exposed to the underlying database technology.

The benefits of using EF Core include increased productivity, reduced development time, and improved code maintainability. It enables developers to write code that is more readable and maintainable, as well as improving the performance of database interactions.

## Enabling a Code-First Approach
One of the unique features of EntityFramework Core is that it allows developers to use a code-first approach, as opposed to a database-first approach. This means that developers can define their domain model in code first, and then EF Core will generate the necessary database schema to match the model.

Using a code-first approach provides several benefits. First, it allows developers to write code in a more natural and intuitive way, focusing on the domain model and business logic, rather than worrying about database schema design. Second, it makes the development process faster and more efficient, as developers can iterate quickly on the code without having to constantly update the database schema. Finally, it makes it easier to maintain the code over time, as any changes to 
the model can be automatically propagated to the database.

## Installing required development tools
Before moving on, make sure you've the `dotnet ef` and `dotnet user-secrets` tools installed globally.  
1. Run `dotnet tool install --global dotnet-ef`
2. Then `dotnet tool install --global dotnet-user-secrets`

## Next up
In the following section of the course, we will cover key concepts in EF Core such as `DbContext`, `IEntityTypeConfiguration<T>`, Converters, Constraints, Migrations, and more.