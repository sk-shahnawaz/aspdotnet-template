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
### Applying Database Migration
Update the Properties\launchSettings.json file with the intended PostgreSQL server connection parameter values under the profile *Localhost*. 
If using Visual Studio, make this project as the start up project & Run the project pointing to the *Localhost* profile. Migration(s) will be applied to the intended PostgreSQL DB instance.

Alternatively, using Terminal, go to the project directory, run the following command:
```
dotnet run --launch-profile Localhost
```

#### launchSettings.json File Connection Parameters for PostgreSQL:
```
{
    "profiles": {
        "Localhost": {
            "commandName": "Project",
            "environmentVariables": {
                "PGHOST": "<PROVIDE POSTGRESQL SERVER NAME HERE>",
                "PGPORT": "<PROVIDE POSTGRESQL SERVER PORT HERE, FOR DOCKERIZED POSTGRES, PROVIDE THE PORT MAPPING INFO FROM DOCKER COMPOSE>",
                "PGPASSWORD": "<PROVIDE POSTGRESQL SERVER USER PASSWORD HERE>",
                "PGUSER": "<PROVIDE POSTGRESQL SERVER USER NAME HERE>",
                "PGDATABASE": "<PROVIDE POSTGRESQL SERVER DATABASE NAME HERE>",
                "PGVERSION": "11.8"
            }
        }
    }
}
```
**Note:** Ensure PostgreSQL instance is running & accessible while running EF Core migration.