using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;

internal static class OpenApiExtension
{
    /// <summary>
    /// Configures Swashbuckle OpenAPI documentation with API versioning support.
    /// </summary>
    /// <param name="serviceCollection">The service collection</param>
    internal static void AddOpenApi(this IServiceCollection serviceCollection)
    {
        var provider = serviceCollection.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();
        serviceCollection.AddSwaggerGen(options =>
        {
            var applicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            foreach (var description in provider!.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.OpenApiInfo
                {
                    Title = applicationName,
                    Version = description.GroupName
                });
            }
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.EnableAnnotations();
        });
        serviceCollection.AddSwaggerGenNewtonsoftSupport();
    }

    /// <summary>
    /// Configures the Swagger UI endpoint for each API version.
    /// </summary>
    internal static void UseSwagger(this IApplicationBuilder applicationBuilder, IApiVersionDescriptionProvider provider)
    {
        applicationBuilder.UseSwagger();
        applicationBuilder.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.ConstructSwaggerEndpoint(description);
            }
        });
    }

    private static void ConstructSwaggerEndpoint(this SwaggerUIOptions options, ApiVersionDescription description)
    {
        options.RoutePrefix = "api-docs";
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        options.DefaultModelExpandDepth(2);
        options.DefaultModelRendering(ModelRendering.Example);
        options.DisplayRequestDuration();
        options.DocExpansion(DocExpansion.List);
        options.EnableDeepLinking();
        options.ShowExtensions();
        options.ShowCommonExtensions();
        options.EnableValidator();
    }
}
