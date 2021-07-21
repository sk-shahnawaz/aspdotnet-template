using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace ASP.NET.Core.WebAPI.Models.DTOs.Contracts
{
	public class LinkDTO
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "href")]
		public string Href { get; private set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "rel")]
		public string Rel { get; private set; }

		[Newtonsoft.Json.JsonProperty(PropertyName = "method")]
		public string Method { get; private set; }

		/// <summary>
		/// This method dynamically generates links to other endpoints of the API, thus making it HATEOAS complaint
		/// </summary>
		/// <param name="urlHelper">Defines the contract for the helper to build URLs for ASP.NET MVC within an application</param>
		/// <param name="id">ID of the resource</param>
		/// <param name="controllerContext">The context associated with the current request for a controller</param>
		/// <returns></returns>
		internal static List<LinkDTO> GenerateHateoasLinks(IUrlHelper urlHelper, long id, ControllerContext controllerContext)
		{
			List<LinkDTO> hateoasLinks = null;
			try
			{
				var httpContext = urlHelper.ActionContext.HttpContext;
				var apiVersion = httpContext.GetRequestedApiVersion();

				List<RouteMetadata> routeMetadatas = GetAllRegisteredRoutes(controllerContext, apiVersion.MajorVersion);
				if (id > 0 && routeMetadatas?.Count > 0)
				{
					hateoasLinks = new();	// C# 9.0
					var linkGenerator = (LinkGenerator)httpContext.RequestServices.GetService(typeof(LinkGenerator));
					foreach (RouteMetadata routeMetadata in routeMetadatas)
					{
						hateoasLinks.Add(new LinkDTO
						{
							Href = linkGenerator.GetUriByAction(httpContext, routeMetadata.ActionName, controllerContext.ActionDescriptor.ControllerName, routeMetadata.Parameters.Any(p => p.Equals(nameof(id))) ? new { id } : null),
							Method = routeMetadata.HttpMethodName.ToUpper(),
							Rel = routeMetadata.ActionName
						});
					}
				}
			}
			catch { }
			return hateoasLinks;
		}

		/// <summary>
		/// This method gets the metadata of a Web API controller through .NET Reflection / Meta-programming
		/// </summary>
		/// <param name="controllerContext">The context associated with the current request for a controller</param>
		/// <param name="apiMajorVersion">API Versioning information</param>
		/// <returns></returns>
		private static List<RouteMetadata> GetAllRegisteredRoutes(ControllerContext controllerContext, int? apiMajorVersion)
		{
			var actionDescriptorCollectionProvider = (IActionDescriptorCollectionProvider)controllerContext.HttpContext.RequestServices.GetService(typeof(IActionDescriptorCollectionProvider));
			var routes = actionDescriptorCollectionProvider.ActionDescriptors.Items.Where
						 (
							ad => ad.AttributeRouteInfo != null && (apiMajorVersion == null || (apiMajorVersion.HasValue && ad.DisplayName.IndexOf($"v{apiMajorVersion.Value}") > -1))).Select(ad => new RouteMetadata
							{
								ActionName = ad.RouteValues["action"].Trim(),
								Parameters = ad.Parameters.Select(param => param.Name),
								HttpMethodName = ad.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods.First(),
							})
						 .ToList();
			return routes;
		}
	}

	internal class RouteMetadata
	{ 
		internal string ActionName { get; set; }
		internal IEnumerable<string> Parameters { get; set; }
		internal string HttpMethodName { get; set; }
	}
}