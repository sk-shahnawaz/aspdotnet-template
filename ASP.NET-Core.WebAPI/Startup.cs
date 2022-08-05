using Microsoft.AspNetCore.Mvc.ApiExplorer;

using ASP.NET.Core.WebAPI.Utilities;
using ASP.NET.Core.WebAPI.Models.UtilityModels;
using ASP.NET.Core.WebAPI.Infrastructure.EFCore;
using ASP.NET.Core.WebAPI.Helpers.Middlewares;
using ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;

namespace ASP.NET.Core.WebAPI;

/// <summary>
/// Startup type used by the web host to configure services and HTTP request processing pipeline.
/// </summary>
public class Startup
{

    #region --- Global Variables ---

    #endregion --- Global Variables ---

    #region --- Properties ---

    /// <summary>
    /// Represents a set of key/value application configuration properties.
    /// </summary>
    public IConfiguration Configuration { get; }

    #endregion --- Properties ---

    #region --- Constructors ---

    /// <summary>
    /// Type constructor to inject application configuration properties.
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    #endregion --- Constructors ---

    #region --- Methods ---

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    /// <param name="services">An abstration representing application services.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        /* 
         * Try not to clutter the method definition by service provider implementations here.
         *  If need to register a service which requires multi-line provider implementation, 
         *  then move provider logic in separate class (Single Responsibilty principle). 
        */

        services.AddOptions(Configuration, out EnvironmentConfiguration environmentConfiguration);
        services.AddOptions(Configuration, out ApplicationConfiguration applicationConfiguration);
        services.AddLogging(environmentConfiguration);
        services.AddCors(environmentConfiguration);
        services.AddRoutingConfigurations();
        services.AddDependencies(environmentConfiguration, applicationConfiguration);
        services.AddModelAutoMappings();
        services.AddVersioning();               // Must come before AddControllers call.
        services.AddWebApi(applicationConfiguration);
        services.AddOpenApi();
        services.AddHealthChecks();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request processing pipeline.
    /// </summary>
    /// <param name="app">An abstraction which provides mechanisms to configure the HTTP request processing pipeline.</param>
    /// <param name="provider">An abstraction representing OpenAPI specification configuration</param>
    /// <param name="environmentConfiguration">Strongly typed environmental configuration</param>
    /// <param name="applicationConfiguration">Strongly typed application configuration</param>
    public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider, EnvironmentConfiguration environmentConfiguration, ApplicationConfiguration applicationConfiguration)
    {
        /*
            While configuring ASP.NET Core pipeline please follow:
            https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0#middleware-order
            Accessed: 2021-06-01
         */

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
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });
        app.UseSwagger(provider);
    }

    #endregion --- Methods ---
}