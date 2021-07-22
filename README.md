# ASP.NET Core Template

The purpose of this repository is to standardize development and deployment workflow for ASP.NET Core based server side development. It contains a sample web API application built with .NET Core.

Best practices related to .NET Development have been followed while developing this template application. It is however, not absolute and any improvement in this template is highly encouraged.
:star: :rocket: :octocat:

## Badges

![build](https://github.com/shahnawaz-qi/aspdotnet-template/actions/workflows/build.yml/badge.svg)

## Contributions

Please read [CONTRIBUTING.md](/CONTRIBUTING.md) before starting to contribute in this project.

## Table of Contents
- [Pre-requisites](#pre-requisites)
- [Structure](#structure)
  - [Solution Projects](#solution-projects)
  - [Repository Root Miscellaneous Items](#repository-root-miscellaneous-items)
- [Third Party Packages Used](#third-party-packages-used)
- [Pre-configured Features](#pre-configured-features)
- [Known Issues](#known-issues)
- [Learning](#learning)

##  Pre-requisites

- .NET 5 (.NET Core 5) SDK & Runtime
- .NET CLI
- EF Core Command Line Tool v.5.0.6+
- PostgreSQL v13.2 / SQL Server 2017 Express Edition with SQL Server Authentication enabled
- Visual Studio 2019 Community Edition v16.10.0+ / Visual Studio Code with appropriate extensions installed
- Docker v20.10.6 build 370c289 or above
- Docker Compose v3.7+

## Structure

###  Solution Projects

| Project Type | Project Name | Usage |
|--------------|--------------|-------|
| ASP.NET Core Web API | [ASP.NET-Core.WebAPI](/ASP.NET-Core.WebAPI) | Main web API project | 
| .NET Core Class Library | [NET-Core.Library.Domain](/NET-Core.Library.Domain) | Application domain logic de-coupled |
| .NET Core Console Application | [NET-Core.Console.DB.PostgreSQL](/NET-Core.Console.DB.PostgreSQL) | Sample PostgreSQL database configurations with Entity Framework (EF) Core migration pre-configured |
| .NET Core Console Application | [NET-Core.Console.DB.SqlServer](/NET-Core.Console.DB.SqlServer) | Sample SQL Server database configurations with Entity Framework (EF) Core migration pre-configured |
| XUnit | [NET-Core.XUnit.UnitTests](/NET-Core.XUnit.UnitTests) | Unit testing project |

### Repository Root Miscellaneous Items

| Item Name | Type | Purpose |
|-----------|------|---------|
| .github | Directory | Contains GitHub Action scripts (CI/CD) |
| .vs | Directory | Contains Visual Studio setting files |
| .dockerignore | File | Contains definitions of files to be ignored by Docker Engine during image building process |
| .editorconfig | File | |
| .gitignore | File | Contains definitions of files to be ignored by Git |
| Scripts | Directory | Contains all scripts related to this repository |
| Scripts\repository-files-renamer.ps1 | File | This Powershell script file can be used to automatically renaming directories/files, replacing file contents to set up a new solution from this template repository |
| CONTRIBUTING.md | File | GitHub repository contributions mark-up file |
| docker-compose.yml | File | Docker compose file to build the application from all dependent images together with networking / volumes etc. |
| Dockerfile | File | Dockerization script file of the application |
| LICENSE.md | File | GitHub repository license mark-up file |
| README.md | File | GitHub repository readme mark-up file |

## Third Party Packages Used

| Project Name | Package Name | Version | License | Usage Area |
|--------------|--------------|---------|---------|------------|	
| ASP.NET-Core.WebAPI |	AutoMapper.Extensions.Microsoft.DependencyInjection	| 8.1.1 | MIT | Dependency Injection for Automapping |
| ASP.NET-Core.WebAPI |	Microsoft.AspNetCore.Mvc.NewtonsoftJson | 5.0.6 | Apache-2.0 | Serialization & Deserialization |
| ASP.NET-Core.WebAPI | Microsoft.AspNetCore.Mvc.Versioning | 5.0.0 | MIT | API versioning |
| ASP.NET-Core.WebAPI | Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer | 5.0.0	| MIT | API versioning, API exploring |
| ASP.NET-Core.WebAPI | Microsoft.EntityFrameworkCore | 5.0.8 | Apache-2.0 | ORM framework |
| ASP.NET-Core.WebAPI | Microsoft.EntityFrameworkCore.Design | 5.0.8 | Apache-2.0 | ORM framework, Schema design |
| ASP.NET-Core.WebAPI | Microsoft.EntityFrameworkCore.InMemory | 5.0.6 | Apache-2.0 | ORM framework, In memory database |
| ASP.NET-Core.WebAPI | Microsoft.VisualStudio.Azure.Containers.Tools.Targets | 1.10.13 | Microsoft Software License |
| ASP.NET-Core.WebAPI | Sentry.Extensions.Logging | 3.4.0 | MIT | Application monitoring |
| ASP.NET-Core.WebAPI | Serilog.Extensions.Logging | 3.0.1 | Apache-2.0 | Logging |
| ASP.NET-Core.WebAPI | Serilog.Settings.Configuration | 3.1.0 | Apache-2.0 | Logging configuration |
| ASP.NET-Core.WebAPI | Serilog.Sinks.Console | 3.1.1 | Apache-2.0 | Console logging |
| ASP.NET-Core.WebAPI | Swashbuckle.AspNetCore | 6.1.4 | MIT | API documentation |
| ASP.NET-Core.WebAPI | Swashbuckle.AspNetCore.Annotations | 6.1.4 | MIT | API documentation annotations |
| ASP.NET-Core.WebAPI | Swashbuckle.AspNetCore.Newtonsoft | 6.1.4 | MIT | Serialization & Deserialization for API documentation |
| ASP.NET-Core.WebAPI | Microsoft.AspNetCore.OData | 7.5.8 | MIT | OData functionality on top of Web API |
| NET.Core.Library.Domain | Microsoft.AspNetCore.Http.Abstractions | 2.2.0 | Apache-2.0 |
| NET.Core.Library.Domain | Microsoft.EntityFrameworkCore | 5.0.6 | Apache-2.0 | ORM framework |
| NET.Core.Library.Domain | Microsoft.EntityFrameworkCore.Design | 5.0.6 | Apache-2.0| ORM framework, Schema design |
| NET.Core.Console.DB.PostgreSQL | Microsoft.EntityFrameworkCore.Design | 5.0.6 | Apache-2.0| ORM dramework, Schema design |
| NET.Core.Console.DB.PostgreSQL | Microsoft.Extensions.Logging.Console | 5.0.0 | MIT | Console logging |
| NET.Core.Console.DB.PostgreSQL | Npgsql.EntityFrameworkCore.PostgreSQL | 5.0.6 | PostgreSQL | ORM framework for PostgreSQL |
| NET.Core.Console.DB.SqlServer | Microsoft.Data.SqlClient | 3.0.0 | Apache-2.0 | ORM framework for SQL Server |
| NET.Core.Console.DB.SqlServer | Microsoft.EntityFrameworkCore.Design | 5.0.8 | MIT | ORM dramework, Schema design |
| NET.Core.Console.DB.SqlServer | Npgsql.EntityFrameworkCore.SqlServer | 5.0.8 | PostgreSQL | ORM framework for SQL Server |
| NET.Core.Console.DB.SqlServer | Microsoft.Extensions.Logging.Console | 5.0.0 | MIT | Console logging |
| NET.Core.XUnit.UnitTests | AutoMapper | 10.1.1 | MIT | Automapping |
| NET.Core.XUnit.UnitTests | coverlet.collector | 3.0.3 | MIT | Mocker for unit testing
| NET.Core.XUnit.UnitTests | Microsoft.NET.Test.Sdk | 16.10.0 | Microsoft Software License	| SDK for Unit testing |		
| NET.Core.XUnit.UnitTests | MockQueryable.Moq | 5.0.1 | MIT | Mocking .NET IQueryable collections  |
| NET.Core.XUnit.UnitTests | Moq | 4.16.1 | BSD 3-Clause | Mocking |
| NET.Core.XUnit.UnitTests | xunit | 2.4.1 | Apache-2.0 | Unit testing |
| NET.Core.XUnit.UnitTests | xunit.runner.visualstudio | 2.4.3 | MIT | Unit testing |
	
## Pre-configured Features:

- Console logging using Serilog
- Application monitoring using Sentry
- HTTPS redirection
- Health check endpoint
- CORS
- Dependency Injection
- EF Core with in-memory database setup or PostgreSQL or SQL Server database setup with Migrations and Seed Data
- Data access from persistence medium through EF Core using Generic Repository Pattern, Unit of Work pattern 
- AutoMapping
- Pagination in API response
- Sample GET/POST/PUT/PATCH/DELETE APIs
- Filtration, Sorting support on GET APIs through OData
- HATEOAS complaint API response
- Consistent & Machine readable error response using ProblemDetails
- API Versioning
- Open API documentation support using Swagger
- Unit Testing setup using XUnit, Moq, Generic Repository Pattern
- Git Ignore, Docker Ignore, Dockerfile and Docker Compose script for containerization
- GitHub actions
- Script to auto renaming components (files/directories, file contents) for setup of new solution from this template.

## Known Issues

- Containairization / Dockerization (TODO)

## Learning

- [C# Extension Methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods)
- [Fundamentals of ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-5.0&tabs=windows)
- [ASP.NET Core WebAPI Sample](https://github.com/FabianGosebrink/ASPNETCore-WebAPI-Sample)
- [Best Practices for REST API Design](https://stackoverflow.blog/2020/03/02/best-practices-for-rest-api-design)
- [Generic Repository Pattern](https://enlabsoftware.com/development/how-to-implement-repository-unit-of-work-design-patterns-in-dot-net-core-practical-examples-part-one.html)
- [.NET CLI](https://docs.microsoft.com/en-us/dotnet/core/tools)
- [Options Pattern in .NET Core - Microsoft Documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0)
- [Options Pattern in .NET Core](https://codeburst.io/options-pattern-in-net-core-a50285aeb18d)