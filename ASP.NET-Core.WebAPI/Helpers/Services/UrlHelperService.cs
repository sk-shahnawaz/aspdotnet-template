using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.NET.Core.WebAPI.Helpers.Services
{
    /// <summary>
    /// This class helps in creating dynamic API endpoint URL during HATEOAS complaint API response generation.
    /// </summary>
	public class UrlHelperService
    {
        public static IUrlHelper GetUrlHelper(IServiceProvider serviceProvider)
        {
            ActionContext actionContext = serviceProvider.GetRequiredService<IActionContextAccessor>().ActionContext;
            IUrlHelperFactory factory = serviceProvider.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(actionContext);
        }		
	}
}