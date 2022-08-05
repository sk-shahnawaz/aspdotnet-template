using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using NET.Core.Library.Domain.Infrastructure;

namespace NET.Core.Console.DB.SqlServer;

class Program
{
    static void Main()
    {
        ILogger<Program> logger = null;
        try
        {
            ServiceProvider serviceProvider = RegisterServices();
            if (serviceProvider != null)
            {
                logger = serviceProvider.GetService<ILogger<Program>>();
                logger.LogInformation("Starting SQL Server database migrations..");

                AppDbContext appSqlServerDbContext = serviceProvider.GetService<AppDbContext>();
                if (appSqlServerDbContext != null && (appSqlServerDbContext is AppSqlServerDbContext))
                {
                    if (appSqlServerDbContext.Database.GetPendingMigrations().Any())
                    {
                        appSqlServerDbContext.Database.Migrate();
                        logger.LogInformation("Migrations applied!");
                    }
                    else
                    {
                        logger.LogWarning("No migrations to apply.");
                    }
                }
                else
                {
                    logger.LogWarning("Failed to get database context.");
                }
            }
        }
        catch (Exception ex)
        {
            if (logger != null)
                logger.LogCritical($"Error message: {ex.Message}, StackTrace: {ex.StackTrace}");
        }
    }

    static ServiceProvider RegisterServices()
    {
        ServiceCollection serviceCollection = new();    
        serviceCollection.AddLogging(options =>
        {
            options.AddConsole()
            .AddSimpleConsole(configure =>
            {
                configure.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
                configure.IncludeScopes = false;
            })
            .SetMinimumLevel(LogLevel.Information);
        });
        serviceCollection.AddSingleton<AppDbContext, AppSqlServerDbContext>();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider;
    }
}