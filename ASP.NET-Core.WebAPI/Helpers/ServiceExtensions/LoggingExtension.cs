using System;
using Serilog;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using ASP.NET.Core.WebAPI.Models.UtilityModels;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions
{
    internal static class LoggingExtension
    {
        /// <summary>
        /// Adds custom logging framework (Serilog), optionally adds application monitoring system (Sentry).
        /// </summary>
        /// <param name="serviceCollection">Abstraction of type IServiceCollection</param>
        /// <param name="environmentConfiguration">A strongly typed instance representing application's environment variables</param>
        internal static void AddLogging(this IServiceCollection serviceCollection, EnvironmentConfiguration environmentConfiguration)
        {
            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);

                if (!string.IsNullOrEmpty(environmentConfiguration?.SENTRY_DSN))
                {
                    loggingBuilder.AddSentry(options =>
                    {
                        options.MinimumEventLevel = Enum.TryParse(typeof(LogLevel), environmentConfiguration.SENTRY_MINIMUM_EVENT_LEVEL, false, out object logLevel) ? (LogLevel)logLevel : LogLevel.Error;
                        options.Dsn = environmentConfiguration.SENTRY_DSN;
                        options.Environment = environmentConfiguration.SENTRY_ENVIRONMENT;
                        options.Release = environmentConfiguration.SENTRY_RELEASE;
                    });
                }
            });
        }
    }
}