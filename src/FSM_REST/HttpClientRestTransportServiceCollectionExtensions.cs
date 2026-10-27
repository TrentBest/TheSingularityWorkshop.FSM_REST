using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Dependency-injection registration helpers for the HTTP REST transport factory.</summary>
public static class HttpClientRestTransportServiceCollectionExtensions
{
    /// <summary>
    /// Registers the transport factory as a singleton. Register configured clients separately with
    /// <c>services.AddHttpClient(name, configureClient)</c>, then request transports from the factory
    /// at the lifetime boundary appropriate to each operation or consumer.
    /// </summary>
    public static IServiceCollection AddHttpClientRestTransportFactory(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddHttpClient();
        services.TryAddSingleton<HttpClientRestTransportFactory>();
        return services;
    }
}
