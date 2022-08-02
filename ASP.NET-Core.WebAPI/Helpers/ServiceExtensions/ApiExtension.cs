using System.Linq;
using System.Net.Mime;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Extensions.DependencyInjection;

using ASP.NET.Core.WebAPI.Models.DTOs;
using ASP.NET.Core.WebAPI.Models.UtilityModels;
using ASP.NET.Core.WebAPI.Infrastructure.API.Filters;
using ASP.NET.Core.WebAPI.Infrastructure.API.Converters;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions
{
    internal static class ApiExtension
    {
        /// <summary>
        /// Adds Web API controllers to the application.
        /// </summary>
        /// <param name="serviceCollection">Abstraction of type IServiceCollection</param>
        /// <param name="applicationConfiguration">Strongly typed instance of Application Configuration</param>
        internal static void AddWebApi(this IServiceCollection serviceCollection, ApplicationConfiguration applicationConfiguration)
        {
            serviceCollection.AddControllers(options => options.Filters.Add(new GlobalErrorResponseFilter()))
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Executes when Model Binding fails for Controllers decorated with APIController attribute.
                    options.InvalidModelStateResponseFactory = (context) =>
                    {
                        ErrorResponse error = new(new ProblemDetails
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Instance = context.HttpContext.TraceIdentifier,
                            Title = Utilities.AppResources.ModelValidationErrorMessage,
                            Detail = "Bad Request"
                        }, context.ModelState);
                        return new ObjectResult(error)
                        {
                            StatusCode = StatusCodes.Status400BadRequest
                        };
                    };
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new DateConverter());
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                    // DefaultContractResolver : Pascal Case : "ThisIsPascalCase"
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            if (applicationConfiguration?.EnableOData ?? false)
            {
                serviceCollection.AddOData();
                serviceCollection.AddMvcCore(options =>
                {
                    // To support Swagger API document generation when OData is included
                    // See: https://github.com/OData/WebApi/issues/1177
                    foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                    {
                        outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                    }
                    foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                    {
                        inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                    }
                });
            }
        }
    }
}