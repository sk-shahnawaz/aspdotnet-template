using System.Linq;
using System.Net.Mime;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Extensions.DependencyInjection;

using ASP.NET.Core.WebAPI.Models.UtilityModels;
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
			serviceCollection.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					// Executes when Model Binding fails for Controllers decorated with APIController attribute.
					options.InvalidModelStateResponseFactory = (context) =>
					{
						ValidationProblemDetails validationProblemDetails = new(context.ModelState)
						{
							Status = Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest,
							Instance = context.HttpContext.TraceIdentifier,
							Title = Utilities.AppResources.ModelValidationErrorMessage,
							Detail = "Bad Request"
						};

						BadRequestObjectResult result = new(validationProblemDetails);  // C# 9.0
						result.ContentTypes.Add(MediaTypeNames.Application.Json);
						return result;
					};
				})
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.Converters.Add(new DateConverter());
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

					// DefaultContractResolver : Pascal Case : "ThisIsPascalCase"
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				})
				.SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

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