using Microsoft.AspNetCore.Cors.Infrastructure;

using ASP.NET.Core.WebAPI.Utilities;
using ASP.NET.Core.WebAPI.Models.UtilityModels;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;

internal static class CorsExtensder
{
    /// <summary>
    /// Adds Cross Origin Resource Sharing (CORS) policy to the application.
    /// </summary>
    /// <param name="serviceCollection">Abstraction of type IServiceCollection</param>
    /// <param name="environmentConfiguration">A strongly typed instance representing application's environment variables</param>
    internal static void AddCors(this IServiceCollection serviceCollection, EnvironmentConfiguration environmentConfiguration)
    {
        string applicationEnvironment = environmentConfiguration?.DOTNET_ENVIRONMENT ?? Environments.Production;
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy(AppResources.CorsPolicyName,
                builder => BuildCorsPolicy(builder, applicationEnvironment));
        });
    }

    /// <summary>
    /// Builds CORS policy according to the environment where application is running. 
    /// </summary>
    /// <param name="corsPolicyBuilder">Builds CORS policy</param>
    /// <param name="applicationEnvironment">Application environment name</param>
    private static void BuildCorsPolicy(this CorsPolicyBuilder corsPolicyBuilder, string applicationEnvironment)
    {
        // Add CORS policy based on Environment by introducing a new switch case. Open-Close principle.
        switch (applicationEnvironment.ToUpper())
        {
            default:
                corsPolicyBuilder
                    .AllowAnyOrigin()
                        .AllowAnyHeader()
                            .AllowAnyMethod(); break;
        }
    }
}