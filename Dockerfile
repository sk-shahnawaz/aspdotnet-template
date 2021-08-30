#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ASP.NET-Core.WebAPI/ASP.NET-Core.WebAPI.csproj", "ASP.NET-Core.WebAPI/"]
RUN dotnet restore "ASP.NET-Core.WebAPI/ASP.NET-Core.WebAPI.csproj"
COPY . .
WORKDIR "/src/ASP.NET-Core.WebAPI"
RUN dotnet build "ASP.NET-Core.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ASP.NET-Core.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASP.NET-Core.WebAPI.dll"]