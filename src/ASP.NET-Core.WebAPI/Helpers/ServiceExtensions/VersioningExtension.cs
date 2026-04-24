using Asp.Versioning;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;

internal static class VersioningExtension
{
    /// <summary>
    /// Adds API versioning (URL path versioning) and API explorer to the application.
    /// </summary>
    /// <param name="serviceCollection">The service collection</param>
    internal static void AddVersioning(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }
}
