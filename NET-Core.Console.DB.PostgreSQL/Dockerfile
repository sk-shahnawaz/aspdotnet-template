#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NET-Core.Library.Domain/NET-Core.Library.Domain.csproj", "NET-Core.Library.Domain/"]
COPY ["NET-Core.Console.DB.PostgreSQL/NET-Core.Console.DB.PostgreSQL.csproj", "NET-Core.Console.DB.PostgreSQL/"]
RUN dotnet restore "NET-Core.Library.Domain/NET-Core.Library.Domain.csproj"
RUN dotnet restore "NET-Core.Console.DB.PostgreSQL/NET-Core.Console.DB.PostgreSQL.csproj"
COPY . .
WORKDIR "/src/NET-Core.Console.DB.PostgreSQL"
RUN dotnet build "NET-Core.Console.DB.PostgreSQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NET-Core.Console.DB.PostgreSQL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NET-Core.Console.DB.PostgreSQL.dll"]