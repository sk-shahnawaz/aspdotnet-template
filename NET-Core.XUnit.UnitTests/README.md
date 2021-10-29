# NET-Core.XUnit.UnitTests

## Overview

This is Unit testing project

## Adding the runsettings file

#### Add the env variables & test run parameters for the testing setting in test.runsettings. reference link is ([here](https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2019))

## Generating Code Coverage Report
The required NuGet packages (coverlet & coverlet.msbuild) are referenced by this project. Issuing a `dotnet restore` command will install them in the local machine.

### Installing _ReportGenerator_ tool globally:
Run Powershell in Administrator mode and execute the following commands:
```powershell
dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools
dotnet new tool-manifest
dotnet tool install dotnet-reportgenerator-globaltool
```

### Adding Visual Studio extension
Head to the Extensions menu and select Manage Extensions. Then, search _Run Coverlet Report_ and install it - you may have to close all Visual Studio instances to install it.

After installation, open Visual Studio and head to Tools > Options and change the Integration Type from Collector to MSBuild.

### Report generation
Under the Tools menu, click on _Run Code Coverage_, this command runs the tests, generates the report file(s) in local system and renders within Visual Studio.

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