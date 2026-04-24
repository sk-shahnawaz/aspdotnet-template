using ASP.NET.Core.WebAPI.Infrastructure.API.RouteConstraints;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;

internal static class RoutingExtension
{
    /// <summary>
    /// Configures the Routing system of the application.
    /// </summary>
    /// <param name="serviceCollection">Abstraction of type IServiceCollection</param>
    internal static void AddRoutingConfigurations(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddRouting(configureOptions =>
        {
            configureOptions.LowercaseUrls = true;

            // Custom Route Constraints registration
            configureOptions.ConstraintMap.Add("email", typeof(EmailConstraint));
        });
    }
}