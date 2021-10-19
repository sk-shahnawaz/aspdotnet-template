using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using NET.Core.Console.DB.PostgreSQL;
using NET.Core.Console.DB.SqlServer;
using ASP.NET.Core.WebAPI.Utilities;
using NET.Core.Library.Domain.Infrastructure;
using ASP.NET.Core.WebAPI.Models.UtilityModels;
using NET.Core.Library.Domain.Infrastructure.Contracts;
using ASP.NET.Core.WebAPI.Helpers.Services;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions
{
    internal static class DependencyExtension
    {
        /// <summary>
        /// Registers depedencies which will be injected by runtime during application's life-time.
        /// </summary>
        /// <param name="serviceCollection">Abstraction of type IServiceCollection</param>
        /// <param name="environmentConfiguration">A strongly typed instance representing application's environment variables</param>
        /// /// <param name="applicationConfiguration">A strongly typed instance representing application's configuration variables (coming from appSettings.json / appSettings.{ENVIRONMENT}.json</param>
        internal static void AddDependencies(this IServiceCollection serviceCollection, EnvironmentConfiguration environmentConfiguration, ApplicationConfiguration applicationConfiguration)
        {
            if (applicationConfiguration?.UseInMemoryDatabase ?? false)
            {
                if (!string.IsNullOrEmpty(applicationConfiguration.IN_MEMORY_DATABASE_NAME))
                {
                    serviceCollection.AddScoped<IAppDbContext, AppDbContext>();
                    serviceCollection.AddDbContext<AppDbContext>(optionsAction =>
                    {
                        optionsAction.UseInMemoryDatabase(applicationConfiguration.IN_MEMORY_DATABASE_NAME);
                        optionsAction.AddDatabaseActionMonitoring(environmentConfiguration.DOTNET_ENVIRONMENT);
                    });
                }
                else
                {
                    throw new System.ArgumentException(AppResources.InMemoryDbNameMissing);
                }
            }
            else
            {
                if (applicationConfiguration?.UsePostgreSql ?? false)
                {
                    #region --- PostgreSQL ---

                    if (AppUtilities.InspectPostgreSQLDbParams(environmentConfiguration))
                    {
                        // Register database context class (The class must inherit AppDbContext) 
                        serviceCollection.AddScoped<IAppDbContext, AppPostgreSQLDbContext>();
                        serviceCollection.AddDbContext<AppPostgreSQLDbContext>(optionsAction =>
                        {
                            optionsAction.AddDatabaseActionMonitoring(environmentConfiguration.DOTNET_ENVIRONMENT);
                        });
                    }
                    else
                    {
                        throw new System.ArgumentException(AppResources.PostgresParamsInvalid);
                    }

                    #endregion --- PostgreSQL ---
                }
                else if (applicationConfiguration?.UseSqlServer ?? false)
                {
                    #region --- Microsoft SQL Server ---

                    if (AppUtilities.InspectSqlServerDbParams(environmentConfiguration))
                    {
                        // Register database context class (The class must inherit AppDbContext) 
                        serviceCollection.AddScoped<IAppDbContext, AppSqlServerDbContext>();
                        serviceCollection.AddDbContext<AppPostgreSQLDbContext>(optionsAction =>
                        {
                            optionsAction.AddDatabaseActionMonitoring(environmentConfiguration.DOTNET_ENVIRONMENT);
                        });
                    }
                    else
                    {
                        throw new System.ArgumentException(AppResources.SqlServerParamsInvalid);
                    }

                    #endregion --- Microsoft SQL Server ---
                }
                else
                {
                    throw new System.ArgumentException(AppResources.DbConfigurationInvalid);
                }
            }

            /* Unit of Work pattern registration */
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

            /* Generic Repository pattern registration */
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            /* Custom service registration */
            serviceCollection.AddScoped<IUrlHelper>(serviceProvider => UrlHelperService.GetUrlHelper(serviceProvider));
        }

        private static void AddDatabaseActionMonitoring(this DbContextOptionsBuilder dbContextOptionsBuilder, string hostingEnvironment)
        {
            if (hostingEnvironment == Environments.Development)
            {
                dbContextOptionsBuilder.EnableSensitiveDataLogging(true);
                dbContextOptionsBuilder.EnableDetailedErrors(true);
                dbContextOptionsBuilder.LogTo(Log.Logger.Information, LogLevel.Information, null);
            }
        }
    }
}