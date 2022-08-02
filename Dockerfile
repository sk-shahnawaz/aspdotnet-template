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

ARG NEW_RELIC_LICENSE_KEY
ARG NEW_RELIC_APP_NAME

# Install the NewRelic agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y newrelic-netcore20-agent

# Enable the NewRelic agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=$NEW_RELIC_LICENSE_KEY \
NEW_RELIC_APP_NAME=$NEW_RELIC_APP_NAME

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASP.NET-Core.WebAPI.dll"]