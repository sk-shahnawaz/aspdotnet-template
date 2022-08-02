using System;
using Serilog;
using System.Collections.Generic;
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

                        // See: https://docs.sentry.io/platforms/dotnet/guides/aspnetcore/configuration/sampling [Accessed on 2022-06-06]
                        options.SampleRate = 0.5f;
                        options.TracesSampler = context =>
                        {
                            // If this is the continuation of a trace, just use that decision (rate controlled by the caller)
                            if (context.TransactionContext.IsParentSampled is not null)
                            {
                                return context.TransactionContext.IsParentSampled.Value
                                    ? 1.0
                                    : 0.0;
                            }

                            // Otherwise, sample based on URL (exposed through custom sampling context)
                            return context.CustomSamplingContext.GetValueOrDefault("url") switch
                            {
                                // The health check endpoint is just noise - drop all transactions
                                "/health" => 0.0,

                                // Open API documentation endpoint
                                "/api-docs/*" => 0.0,

                                // Default sample rate
                                _ => 0.3
                            };
                        };
                    });
                }
            });
        }
    }
}