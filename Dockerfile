FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/ASP.NET-Core.WebAPI/ASP.NET-Core.WebAPI.csproj", "src/ASP.NET-Core.WebAPI/"]
COPY ["src/NET-Core.Library.Domain/NET-Core.Library.Domain.csproj", "src/NET-Core.Library.Domain/"]
COPY ["src/NET-Core.Console.DB.PostgreSQL/NET-Core.Console.DB.PostgreSQL.csproj", "src/NET-Core.Console.DB.PostgreSQL/"]
COPY ["src/NET-Core.Console.DB.SqlServer/NET-Core.Console.DB.SqlServer.csproj", "src/NET-Core.Console.DB.SqlServer/"]
RUN dotnet restore "src/ASP.NET-Core.WebAPI/ASP.NET-Core.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/ASP.NET-Core.WebAPI"
RUN dotnet build "ASP.NET-Core.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ASP.NET-Core.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASP.NET-Core.WebAPI.dll"]
