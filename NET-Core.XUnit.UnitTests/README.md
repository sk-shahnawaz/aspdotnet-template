# NET-Core.XUnit.UnitTests

## Overview

This is Unit testing project

## Adding the runsettings file

#### Add the env variables & test run parameters for the testing setting in test.runsettings. reference link is ([here](https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2019))

## Build & Run Process 	

#### Move To Project Directory
```
cd NET-Core.XUnit.UnitTests
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

#### Run Test
```
dotnet test --settings "test.runsettings" --configuration [Release/Debug]
```