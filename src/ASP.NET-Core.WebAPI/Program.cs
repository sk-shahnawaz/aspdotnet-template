using Serilog;
using Serilog.Formatting.Json;
using ASP.NET.Core.WebAPI.Models.UtilityModels;
using ASP.NET.Core.WebAPI.Utilities;
using ASP.NET.Core.WebAPI.Helpers.Middlewares;
using ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;
using ASP.NET.Core.WebAPI.Infrastructure.EFCore;

namespace ASP.NET.Core.WebAPI;

/// <summary>
/// Entry point of the ASP.NET Core application.
/// </summary>
public class Program
{
    /// <summary>
    /// Configures and runs the web application.
    /// </summary>
    /// <param name="args">Command-line arguments</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptions(builder.Configuration, out EnvironmentConfiguration environmentConfiguration);
        builder.Services.AddOptions(builder.Configuration, out ApplicationConfiguration applicationConfiguration);
        builder.Services.AddLogging(environmentConfiguration);
        builder.Services.AddCors(environmentConfiguration);
        builder.Services.AddRoutingConfigurations();
        builder.Services.AddDependencies(environmentConfiguration, applicationConfiguration);
        builder.Services.AddModelAutoMappings();
        builder.Services.AddVersioning();
        builder.Services.AddWebApi(applicationConfiguration);
        builder.Services.AddOpenApi();
        builder.Services.AddHealthChecks();

        InitializeConsoleLogging(builder.Configuration, environmentConfiguration);

        var app = builder.Build();

        if (applicationConfiguration?.UseInMemoryDatabase ?? false)
        {
            InMemoryDbDataSeeder.SeedTestData(app);
        }

        if (environmentConfiguration?.DOTNET_ENVIRONMENT == Environments.Development)
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors(AppResources.CorsPolicyName);
        app.UseMiddleware<RequestResponseLogger>();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health");
        app.UseSwagger(app.Services.GetRequiredService<Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider>());

        Log.Information("ASP.NET Core Web API application running..");
        app.Run();
    }

    /// <summary>
    /// Initializes Serilog console logging with the configured minimum log level.
    /// </summary>
    private static void InitializeConsoleLogging(IConfiguration configuration, EnvironmentConfiguration? environmentConfiguration)
    {
        if (!Enum.TryParse(environmentConfiguration?.SERILOG_LOGGING_LEVEL, ignoreCase: true, out Serilog.Events.LogEventLevel minLogEventLevel))
        {
            minLogEventLevel = Serilog.Events.LogEventLevel.Warning;
        }
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel
            .ControlledBy(new Serilog.Core.LoggingLevelSwitch(minLogEventLevel))
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console(formatter: new JsonFormatter())
            .CreateLogger();
    }
}
