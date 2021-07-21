# NET-Core.Console.DB.SqlServer

## Overview

This is project for running SQL Server DB migrations as console application.

## Project structure
| Directory Name | Usage |
|----------------|-------|
| Migrations | Contains all the Entity Framework Core migration files |
| Models\Maps | Contains database specific versions of Domain Model classes for SQL Server database |

### Environment Variables

| Variable Name | Help Text |
|---------------|-----------|
| SQLHOST | SQL Server host name |
| SQLPORT | SQL Server port number |
| SQLPASSWORD | SQL Server password |
| SQLUSER | SQL Server user name |
| SQLDATABASE | SQL Server database name |

## Build & Run Process 	

#### Move To Project Directory
```
cd NET-Core.Console.DB.SqlServer
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
