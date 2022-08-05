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
- [Pre-configured Features](#pre-configured-features)
- [Known Issues](#known-issues)
- [Learning](#learning)

##  Pre-requisites

- Git Version Control System
- .NET 6 SDK & Runtime
- .NET CLI v 6.0.0+
- EF Core Command Line Tool v 6.0.0+
- PostgreSQL v13.2 / SQL Server 2017 Express Edition+ with SQL Server Authentication enabled
- Visual Studio 2022 Community Edition / Visual Studio Code with OmniSharp extension installed / JetBrains Rider
- Docker v20.10.6 build 370c289 or above
- Docker Compose v3.7+
- Additional setup needed for generating unit test coverage reports [described here](/NET-Core.XUnit.UnitTests#generating-code-coverage-report)

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
| docker-compose.yml | File | Docker compose file to build the application docker image along with PostgreSQL image, PostgreSQL specific migrations and with networking / volumes etc. |
| Dockerfile | File | Dockerization script file of the application |
| LICENSE.md | File | GitHub repository license mark-up file |
| README.md | File | GitHub repository readme mark-up file |
	
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
- Sentry & New Relic integration support
- Unit Testing setup using XUnit, Moq, Generic Repository Pattern
- Code Coverage Report Generation setup with report summary in GitHub PR as Bot Comment
- Git Ignore, Docker Ignore, Dockerfile and Docker Compose script for containerization
- GitHub actions
- Script to auto renaming components (files/directories, file contents) for setup of new solution from this template.

## Known Issues

- Containairization / Dockerization with HTTPS certification and PostgreSQL image
- Containairization / Dockerization with HTTPS certification and SQL Server image

## Learning

- [C# Extension Methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods)
- [Fundamentals of ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-5.0&tabs=windows)
- [ASP.NET Core WebAPI Sample](https://github.com/FabianGosebrink/ASPNETCore-WebAPI-Sample)
- [Best Practices for REST API Design](https://stackoverflow.blog/2020/03/02/best-practices-for-rest-api-design)
- [Generic Repository Pattern](https://enlabsoftware.com/development/how-to-implement-repository-unit-of-work-design-patterns-in-dot-net-core-practical-examples-part-one.html)
- [.NET CLI](https://docs.microsoft.com/en-us/dotnet/core/tools)
- [Options Pattern in .NET Core - Microsoft Documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0)
- [Options Pattern in .NET Core](https://codeburst.io/options-pattern-in-net-core-a50285aeb18d)
- [Code Coderage in .NET using Coverlet](https://github.com/coverlet-coverage/coverlet)
- [Publish Code Coverage Summary to GitHub PR as Bot Comment](https://josh-ops.com/posts/github-code-coverage)