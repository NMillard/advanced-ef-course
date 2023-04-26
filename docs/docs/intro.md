---
title: Introduction - Start here
slug: /
sidebar_position: 1
---

# Introduction
By the end of this course, you'll have comprehensive understanding of EntityFramework Core and be able to use it effectively in real-world software development projects. You will be able to:
- [x] Use proper encapsulation,
- [x] Configure advanced entity settings
- [x] Use testcontainers to test with a real database.

### What you'll need
Make sure to have installed the following:

- .NET 7 SDK
- nodejs 16 or above
- Have a .NET capable IDE install (JetBrains Rider, Visual Studio, VS Code)
- Docker and Docker-compose

### Start an MS-SQL Server
If you don't already have an MS-SQL server running on your computer, then run the docker-compose file to have one started on port `1499` for you.

From the root of the solution folder, run `docker-compose up`. Then connect to the database server and create a database called `AdvancedORM`.