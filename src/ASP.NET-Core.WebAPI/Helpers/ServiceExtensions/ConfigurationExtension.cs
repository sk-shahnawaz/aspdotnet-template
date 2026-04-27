using Microsoft.Extensions.Options;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;

internal static class ConfigurationExtension
{
    /// <summary>
    /// Provides strongly typed access to configuration parameters / environment variables.
    /// </summary>
    /// <typeparam name="T">T represents class whose bounded instance will be emitted</typeparam>
    /// <param name="serviceCollection">Abstraction of type IServiceCollection</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties</param>
    /// <param name="stronglyTypedConfiguration">out parameter, representing an instance of type T emitted for immediate use</param>
    internal static void AddOptions<T>(this IServiceCollection serviceCollection, IConfiguration configuration, out T stronglyTypedConfiguration)
        where T : class
    {
        serviceCollection.AddOptions<T>().Bind(configuration);

        // This provider implementation will be used by the runtime when application is running
        serviceCollection.AddSingleton<T>(serviceProvider => ImplementationFactory<T>(serviceProvider));

        // Immediately invoking to grab a strongly typed instance of Environment Variables,
        // this will be used in the pipeline for registering other services
        stronglyTypedConfiguration = serviceCollection.BuildServiceProvider().GetService<IOptions<T>>()?.Value as T;
    }

    /// <summary>
    /// Returns an instance (T) representing a strongly typed configuration
    /// </summary>
    /// <typeparam name="T">Generic type argument, representing the type to be returned from application's service collection</typeparam>
    /// <param name="serviceProvider">Abstraction of type IServiceCollection</param>
    /// <returns></returns>
    internal static T ImplementationFactory<T>(IServiceProvider serviceProvider)
        where T : class
    {
        T stronglyTypedConfiguration = serviceProvider.GetService<IOptions<T>>()?.Value as T;
        return stronglyTypedConfiguration;
    }
}