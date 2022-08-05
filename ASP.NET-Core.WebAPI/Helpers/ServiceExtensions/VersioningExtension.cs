using Microsoft.AspNetCore.Mvc;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;

internal static class VersioningExtension
{
    /// <summary>
    /// Adds API versioning functionality (in the form of URL path versioning scheme) to the application.
    /// </summary>
    /// <param name="serviceCollection">Abstraction of type IServiceCollection</param>
    internal static void AddVersioning(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddApiVersioning(setupAction =>
        {
            setupAction.ReportApiVersions = true;
            setupAction.AssumeDefaultVersionWhenUnspecified = true;
            setupAction.DefaultApiVersion = new ApiVersion(1, 0);
        });
        serviceCollection.AddVersionedApiExplorer(setupAction =>
        {
            setupAction.GroupNameFormat = "'v'VVV";

            // This option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            setupAction.SubstituteApiVersionInUrl = true;
        });
    }
}