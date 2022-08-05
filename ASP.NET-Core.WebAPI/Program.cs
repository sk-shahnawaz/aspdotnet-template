using Microsoft.Extensions.Options;

using Serilog;
using Serilog.Formatting.Json;

using ASP.NET.Core.WebAPI.Models.UtilityModels;

namespace ASP.NET.Core.WebAPI;

public class Program
{
    /// <summary>
    /// Entry point of the ASP.NET Core application. Configures the WebHost, builds it and runs the application.
    /// </summary>
    /// <param name="args">Command line argument(s)</param>
    public static void Main(string[] args)
    {
        IHost host = null;
        try
        {
            host = CreateHostBuilder(args).Build();
            if (host != null)
            {
                InitializeConsoleLogging(host);
                Log.Logger.Information("ASP.NET Core Web API application running..");
                host.Run();
            }
        }
        catch (Exception ex)
        {
            if (Log.Logger != null)
                Log.Logger.Fatal("Fatal error encountered in ASP.NET Core Web API application : {@data}", ex);
        }
        finally
        {
            if (host != null)
            {
                host.Dispose();
            }
        }
    }

    /// <summary>
    /// Initializes the custom logging framework (Serilog) to use the minimum logging level and do console logging.
    /// </summary>
    /// <param name="host">A program abstraction</param>
    private static void InitializeConsoleLogging(IHost host)
    {
        EnvironmentConfiguration environmentConfiguration = host.Services.GetService<IOptions<EnvironmentConfiguration>>()?.Value;
        if (!Enum.TryParse(environmentConfiguration?.SERILOG_LOGGING_LEVEL, ignoreCase: true, out Serilog.Events.LogEventLevel minLogEventLevel))
        {
            minLogEventLevel = Serilog.Events.LogEventLevel.Warning;
        }
        Log.Logger = new LoggerConfiguration()
                            .MinimumLevel
                                .ControlledBy(levelSwitch: new Serilog.Core.LoggingLevelSwitch(minLogEventLevel))
                                    .ReadFrom.Configuration(host.Services.GetRequiredService<IConfiguration>())
                                        .WriteTo.Console(formatter: new JsonFormatter())
                                            .CreateLogger();
    }

    /// <summary>
    /// Creates the web host, loads configuration settings from different resources in the following order:
    /// <para>1. AppSettings.json</para>
    /// <para>2. AppSettings.{ENVIRONMENT}.json</para>
    /// <para>3. User secrets file (only in 'Development' environment)</para>
    /// <para>4. Environment variables</para>
    /// <para>5. Command line arguments</para>
    /// Among other things, it sets up the default logging framework and web server accordingly:
    /// <para>Command=PROJECT; HOSTING MODEL: *ignored*; Server(s): KESTREL (Recommended for deployment as a Microservice in Cloud)</para>
    /// <para>Command=IISEXPRESS; HOSTING MODEL: INPROCESS; Server(s): IISEXPRESS</para>
    /// <para>Command=IISEXPRESS; HOSTING MODEL: OUTOFPROCESS; Server(s): KESTREL (internal) | IISEXPRESS (external)</para>
    /// <para>Command=IIS; HOSTING MODEL: INPROCESS; Server(s): IIS</para>
    /// <para>Command=IIS; HOSTING MODEL: OUTOFPROCESS; Server(s): KESTREL (internal) | IISEXPRESS (external)</para>
    /// </summary>
    /// <param name="args">Command line argument(s)</param>
    /// <returns>An abstraction representing the web host.</returns>
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}