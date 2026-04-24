namespace ASP.NET.Core.WebAPI.Models.UtilityModels;

/// <summary>
/// This type represents all the environment variables of the application.
/// It is used to bind the configuration so as to get a strongly typed instance
/// of the application's environment variables. Ensure access specifiers are 
/// made public, else DI system won't be able to bind the properties.
/// </summary>
public class EnvironmentConfiguration
{
    public string DOTNET_ENVIRONMENT { get; init; } = Environments.Production;

    /* Logging - Serilog */
    public string SERILOG_LOGGING_LEVEL { get; init; } = "Error";
    public string SENTRY_MINIMUM_EVENT_LEVEL { get; init; } = "Error";

    /* Monitoring - Sentry */
    public string SENTRY_DSN { get; init; }
    public string SENTRY_ENVIRONMENT { get; init; } = "local";
    public string SENTRY_RELEASE { get; init; } = "local-version";

    /* Database - PostgreSQL */
    public string PGHOST { get; init; }
    public string PGPORT { get; init; }
    public string PGDATABASE { get; init; }
    public string PGUSER { get; init; }
    public string PGPASSWORD { get; init; }
    public string PGVERSION { get; init; }

    /* Database - SQL Server */
    public string SQLHOST { get; init; }
    public string SQLPORT { get; init; }
    public string SQLDATABASE { get; init; }
    public string SQLUSER { get; init; }
    public string SQLPASSWORD { get; init; }
}