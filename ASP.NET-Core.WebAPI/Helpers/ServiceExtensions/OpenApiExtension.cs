using System.Linq;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions
{
    internal static class OpenApiExtension
    {
        /// <summary>
        /// Configures and adds Swashbuckle OpenAPI Documentation service with versioning support.
        /// </summary>
        /// <param name="serviceCollection">Abstraction of type IServiceCollection</param>
        internal static void AddOpenApi(this IServiceCollection serviceCollection)
        {
            IApiVersionDescriptionProvider provider = serviceCollection.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();
            serviceCollection.AddSwaggerGen(setupAction =>
            {
                string applicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    setupAction.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = applicationName,
                        Version = description.GroupName
                    });
                }
                setupAction.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                setupAction.EnableAnnotations();
            });
            // See: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#systemtextjson-stj-vs-newtonsoft
            serviceCollection.AddSwaggerGenNewtonsoftSupport();
        }

        /// <summary>
        /// Configures Swashbuckle OpenAPI endpoint for different versions of the API.
        /// </summary>
        /// <param name="applicationBuilder">Defines a class that provides mechanism for configuring application's pipeline</param>
        /// <param name="provider">Defines the behavior of a provider that discovers and describes API version information within an application.</param>
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

        /// <summary>
        /// Configures a single Swashbuckle OpenAPI endpoint for specific version of the API.
        /// </summary>
        /// <param name="swaggerUiOptions">Option to configure SwaggerUI</param>
        /// <param name="apiVersionDescription">Instance of 'ApiVersionDescription' representing of an API version</param>
        internal static void ConstructSwaggerEndpoint(this SwaggerUIOptions swaggerUiOptions, ApiVersionDescription apiVersionDescription)
        {
            swaggerUiOptions.RoutePrefix = "api-docs";
            swaggerUiOptions.SwaggerEndpoint($"/swagger/{apiVersionDescription.GroupName}/swagger.json", apiVersionDescription.GroupName.ToUpperInvariant());
            swaggerUiOptions.DefaultModelExpandDepth(2);
            swaggerUiOptions.DefaultModelRendering(ModelRendering.Example);
            swaggerUiOptions.DisplayRequestDuration();
            swaggerUiOptions.DocExpansion(DocExpansion.List);
            swaggerUiOptions.EnableDeepLinking();
            swaggerUiOptions.ShowExtensions();
            swaggerUiOptions.ShowCommonExtensions();
            swaggerUiOptions.EnableValidator();
        }
    }
}