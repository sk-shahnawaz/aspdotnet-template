# NET-Core.Console.DB.PostgreSQL

## Overview

This is project for running PostgresSQL DB migrations as console application.

## Project Structure
| Directory Name | Usage |
|----------------|-------|
| Migrations | Contains all the Entity Framework Core migration files |
| Models\Maps | Contains database specific versions of Domain Model classes for PostgreSQL database |

### Environment Variables

| Variable Name | Help Text |
|---------------|-----------|
| PGHOST | PostgreSQL host name |
| PGPORT | PostgreSQL port number |
| PGPASSWORD | PostgreSQL password |
| PGUSER | PostgreSQL user name |
| PGDATABASE | PostgreSQL database name |
| PGVERSION | PostgreSQL version |

## Build & Run Process 	

#### Move To Project Directory
```
cd NET-Core.Console.DB.PostgreSQL
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
