# Project description

This project simulates a blogging database.  
We'll work with classes such as User, Author, Article, Comments.

## Prerequisites

- .NET 7 SDK
- install dotnet-ef, dotnet-user-secrets

## Outline

- Introduction to EntityFramework Core and the benefits it brings
- Query Examples
- Key Concepts and setup
    - DbContext
    - IEntityTypeConfiguration
    - Converters
    - Constraints, primary and foreign key, CHECK
    - Indexes
    - Joins, 1-to-1, 1-to-many, many-to-many (join table)
- Models
    - Attributes
- Code First database management
- Separating infrastructure from business logic
- Soft delete
  - Global filter for type
- Storing secrets such as the connection string
- Repositories
  - Partial hydration
  - Auto include
- Adding data on migrate (separate migrations)
- Testing with an actual database

## Domain overview

In simple terms, an author profile may be shared among many users but must always be administered by one. An author
writes and publishes articles. Users may comment on published articles.

```mermaid
erDiagram
    User {
        guid id PK
    }

    User ||--|| UserSettings: Has
    UserSettings {
        guid id PK
        guid user_id FK
    }

    User ||--o{ AuthorProfile: "can have many profiles"
    Author ||--o{ AuthorProfile: "shared with users"
    AuthorProfile {
        guid user_id FK
        guid author_id FK
        bool isAdministrator FK "User id"
    }

    Author {
        guid id PK
    }

    Author ||--o{ Article: Publish
    Article {
        guid id PK
        guid author_id FK
    }

    Response {
        guid id PK
        guid article_id FK
    }
    Response }o--|| Article: Has
```

### Article

State diagram showing how an article can switch state.

## Notes

- Neglecting database knowledge and techniques is a mistake. To use any ORM effectively, you need to know the underlying
  technology intimately.
- Removing persistence logic from entities
- Generating primary keys: GUID vs Long, Application vs Database generated
- Model attributes inconvenient conveniences

## Exercise

Generate migration from unconfigured classes. Inspect the proposed changes from EF Core. Tweak the configuration.
Run generate SQL script.

## Links

- https://www.dataversity.net/a-short-history-of-the-er-diagram-and-information-modeling/#
- https://creately.com/guides/er-diagrams-tutorial/
- https://vertabelo.com/blog/vertabelo-tips-good-er-diagram-layout/
- https://learn.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=fluent-api

## Structure
Section 1: Introduction to OR/Ms and Benefits

- Object Relation Mapping (OR/M) explained
- Benefits of using an OR/M
- Why choose EntityFramework Core over other OR/Ms

Section 2: Key Concepts in EF Core

- DbContext explained
- IEntityTypeConfiguration<T>
- Converts, Constraints, and Migrations
- Code first vs. Database first

Section 3: Relationships in EF Core

    Joins in EF Core
    One-to-one relationships
    One-to-many relationships
    Many-to-many relationships

Section 4: Encapsulating Models and Class Standards

    Encapsulating models for better structure
    Good class standards for EF Core

Section 5: Securely Storing Connection Strings

    How to securely store connection strings in production environments
    Best practices for connection string management

Section 6: Advanced Entity Configuration

    Soft delete in EF Core
    Adding timestamps to entities
    Auto includes in EF Core

Section 7: Adding Data on Migrate

    Adding data when creating a database
    Creating seed data in EF Core

Section 8: Testing with a Real Database using Testcontainers

    How to test your EF Core application with a real database using testcontainers
    Best practices for testing EF Core applications

Overall Objective:
By the end of this course, students will have a comprehensive understanding of EntityFramework Core and be able to use it effectively in real-world software development projects. They will be able to encapsulate models, configure advanced entity settings, and use testcontainers to test their applications with a real database.