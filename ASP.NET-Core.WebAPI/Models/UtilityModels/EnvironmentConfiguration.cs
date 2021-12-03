namespace ASP.NET.Core.WebAPI.Models.UtilityModels
{
    /// <summary>
    /// This type represents all the environment variables of the application.
    /// It is used to bind the configuration so as to get a strongly typed instance
    /// of the application's environment variables. Ensure access specifiers are 
    /// made public, else DI system won't be able to bind the properties.
    /// </summary>
    public class EnvironmentConfiguration
    {
        public string DOTNET_ENVIRONMENT { get; set; } = Microsoft.Extensions.Hosting.Environments.Production;

        /* Logging - Serilog */
        public string SERILOG_LOGGING_LEVEL { get; set; } = "Error";
        public string SENTRY_MINIMUM_EVENT_LEVEL { get; set; } = "Error";

        /* Monitoring - Sentry */
        public string SENTRY_DSN { get; set; }
        public string SENTRY_ENVIRONMENT { get; set; } = "local";
        public string SENTRY_RELEASE { get; set; } = "local-version";

        /* Database - PostgreSQL */
        public string PGHOST { get; set; }
        public string PGPORT { get; set; }
        public string PGDATABASE { get; set; }
        public string PGUSER { get; set; }
        public string PGPASSWORD { get; set; }
        public string PGVERSION { get; set; }

        /* Database - SQL Server */
        public string SQLHOST { get; set; }
        public string SQLPORT { get; set; }
        public string SQLDATABASE { get; set; }
        public string SQLUSER { get; set; }
        public string SQLPASSWORD { get; set; }
    }
}