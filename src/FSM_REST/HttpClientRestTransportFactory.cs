namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Creates HTTP-backed REST transports from host-configured <see cref="IHttpClientFactory"/> clients.</summary>
/// <remarks>
/// Each returned transport owns and disposes the <see cref="HttpClient"/> instance returned by the factory.
/// The underlying handler lifetime remains managed by <see cref="IHttpClientFactory"/>. Configure named clients
/// through the host's dependency-injection registration; this factory does not add authentication or retry policy.
/// </remarks>
public sealed class HttpClientRestTransportFactory
{
    private readonly IHttpClientFactory _clientFactory;

    /// <summary>Creates a transport factory using the host's HTTP client factory.</summary>
    public HttpClientRestTransportFactory(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
    }

    /// <summary>Creates a transport using the default HTTP client configuration.</summary>
    public HttpClientRestTransport Create() => Create(name: null);

    /// <summary>Creates a transport using the selected named HTTP client configuration.</summary>
    /// <param name="name">Name registered with <c>AddHttpClient</c>, or null for the default client.</param>
    public HttpClientRestTransport Create(string? name)
    {
        var client = name is null
            ? _clientFactory.CreateClient()
            : _clientFactory.CreateClient(name);

        return new HttpClientRestTransport(client, ownsClient: true);
    }
}
