# ASP.NET-Core.WebAPI

## Overview

This is main web API project containing all the API resources.

## Project structure

| Directory Name | Usage |
|----------------|-------|
| Controllers | Contains all the API controllers with versioning (v1, v2) |
| Helpers\Middlewares | Contains all the ASP.NET Core middlewares for intercepting HTTP request / HTTP response object
| Helpers\ServiceExtensions | Contains all the service registrations of the application (in the form of C# extension methods, following builder pattern), these are used during pipeline construction |
| Helpers\Services | Contain all other services of the application |
| Infrastructure\API\Converters | Contains all the Web API converters, used to convert route parameters during request / response processing |
| Infrastructure\API\RouteConstaints | Contains all the Web API route constraints, used to filter out bad requests from hitting controller action |
| Infrastructure\API\Validators | Contains all the custom validator attributes used during model binding |
| Infrastructure\EFCore | Constains Entity Framework Core specific files (Seeding data) |
| Models\DTOs\Contracts | Contains contract definition to be used by all DTOs |
| Models|DTOs | Contains all the DTOs of the application |
| Models\Utilites | Contain other classes used in the project |
| Utilities | Contain utility classes and methods used in the project |

## Application Configuration

### AppSettings.json

- **USE_IN_MEMORY_DATABASE** [true/false], when set to true, application uses in-memory database, when set to false, tries to configure and use PostgreSQL database
- **IN_MEMORY_DATABASE_NAME**, Name of the in-memory database, considered only when '*USE_IN_MEMORY_DATABASE*' is set to *true*
- **USE_POSTGRESQL_DB** [true/false], when set to true, application configures and uses PostgreSQL as persisitent medium, note that this works only when '*USE_IN_MEMORY_DATABASE*' is set to *false*
- **USE_SQLSERVER_DB** [true/false], when set to true, application configures and uses Microsoft SQL Server as persisitent medium, note that this works only when '*USE_IN_MEMORY_DATABASE*' is set to *false*, furthermore, when both PostgrSQL & SQL Server are enabled, PostgreSQL will get higher preference
- **ENABLE_ODATA** [true/false], when set to true, application adds OData functionality for selection, sorting on top of Web API, when set to false, disables this feature

### Environment Variables

| Variable Name | Help Text |
|---------------|-----------|
| SENTRY_RELEASE | Sentry release number |
| SENTRY_MINIMUM_EVENT_LEVEL | Minimum logging level for Sentry |
| SERILOG_LOGGING_LEVEL | Minimum logging level for Serilog |
| SENTRY_ENVIRONMENT | Sentry environment |
| ASPNETCORE_ENVIRONMENT | ASP.NET Core environment |
| SENTRY_DSN | Sentry DSN |
| PGHOST | PostgreSQL host name |
| PGPORT | PostgreSQL port number |
| PGPASSWORD | PostgreSQL password |
| PGUSER | PostgreSQL user name |
| PGDATABASE | PostgreSQL database name |
| PGVERSION | PostgreSQL version |

## Build & Run Process 	

#### Move To Project Directory
```
cd ASP.NET-Core.WebAPI
```

#### Clean
```
dotnet clean
```

#### Restore
```
dotnet restore --force --no-cache
```

#### Build
```
dotnet build --configuration [Release/Debug] --no-restore
```

#### Run
```
dotnet run
```

#### Browse API-Docs

Navigate to the https://{HOST_URL}/api-docs/index.html to view the API documentation

## Deployment Process

#### Move To Solution Directory
```
cd aspdotnet-template
```

#### Docker Compose
```
docker-compose --project-name aspnet-core-webapi-template-cluster --verbose --file docker-compose.yml up --build
```

## Learning

### Scope Handling in ASP.NET Core

If, from a controller action, an operation (method) is invoked on a Thread Pool Task to run as a background work, then by the time
control comes in that method, the caller controller action method finishes executing and thus runtime disposes all dependent objects
like database context. Iin order to use such dependent objects we have to pass an instance of IServiceScopeFactory and then obtain a new
scope which then can be used to get the required dependent object. 

Visit: https://stackoverflow.com/q/39109234/4287015 for more (Accessed on 2021-05-07).