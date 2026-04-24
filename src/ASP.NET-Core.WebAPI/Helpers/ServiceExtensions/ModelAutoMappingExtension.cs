namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;

internal static class ModelAutoMappingExtension
{
    /// <summary>
    /// Registers AutoMapper mapping profiles.
    /// </summary>
    /// <param name="serviceCollection">The service collection</param>
    internal static void AddModelAutoMappings(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
    }
}
