using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NET.Core.Console.DB.PostgreSQL;
using NET.Core.Console.DB.SqlServer;
using ASP.NET.Core.WebAPI.Models.DTOs;
using NET.Core.Library.Domain.DBModels;
using ASP.NET.Core.WebAPI.Utilities;
using NET.Core.Library.Domain.Infrastructure;
using ASP.NET.Core.WebAPI.Models.UtilityModels;
using NET.Core.Library.Domain.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using ASP.NET.Core.WebAPI.Helpers.Services;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions
{
    internal static class DependencyExtension
    {
        /// <summary>
        /// Registers depedencies which will be injected by runtime during application's life-time. Registers AutoMapper mapping profiles.
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
                        optionsAction.EnableSensitiveDataLogging(false);
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
                        serviceCollection.AddScoped<IAppDbContext>(provider => new AppPostgreSQLDbContext(environmentConfiguration?.PGHOST, environmentConfiguration?.PGPORT, environmentConfiguration?.PGDATABASE, environmentConfiguration?.PGUSER, environmentConfiguration?.PGPASSWORD, environmentConfiguration?.PGVERSION));
                        serviceCollection.AddDbContext<AppPostgreSQLDbContext>(optionsAction =>
                        {
                            optionsAction.EnableSensitiveDataLogging(false);
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
                        serviceCollection.AddScoped<IAppDbContext>(provider => new AppSqlServerDbContext(environmentConfiguration?.SQLHOST, environmentConfiguration?.SQLPORT, environmentConfiguration?.SQLDATABASE, environmentConfiguration?.SQLUSER, environmentConfiguration?.SQLPASSWORD));
                        serviceCollection.AddDbContext<AppSqlServerDbContext>(optionsAction =>
                        {
                            optionsAction.EnableSensitiveDataLogging(false);
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
    }
}