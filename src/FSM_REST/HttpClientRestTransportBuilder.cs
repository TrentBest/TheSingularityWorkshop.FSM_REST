using Microsoft.Extensions.Http;

namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Builds an <see cref="HttpClientRestTransport"/> with explicit client ownership.</summary>
/// <remarks>
This builder configures a client it creates, accepts a caller-owned client without mutating it,
or obtains a named client from <see cref="IHttpClientFactory"/>. A built transport should be disposed
when its lifetime ends; caller-owned clients are never disposed by the transport.
</remarks>
public sealed class HttpClientRestTransportBuilder
{
    private HttpClient? _providedClient;
    private HttpMessageHandler? _handler;
    private bool _disposeHandler = true;
    private IHttpClientFactory? _clientFactory;
    private string? _clientName;
    private Action<HttpClient>? _configureClient;

    /// <summary>Creates a builder that will create and own a new <see cref="HttpClient"/>.</summary>
    public static HttpClientRestTransportBuilder Create() => new();

    /// <summary>Uses a client supplied and owned by the caller.</summary>
    public HttpClientRestTransportBuilder UseClient(HttpClient client)
    {
        _providedClient = client ?? throw new ArgumentNullException(nameof(client));
        _handler = null;
        _clientFactory = null;
        _clientName = null;
        return this;
    }

    /// <summary>Creates a client around a caller-supplied handler.</summary>
    /// <param name="handler">Handler used by the new client.</param>
    /// <param name="disposeHandler">Whether disposing the built transport also disposes the handler.</param>
    public HttpClientRestTransportBuilder UseHandler(HttpMessageHandler handler, bool disposeHandler = true)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _disposeHandler = disposeHandler;
        _providedClient = null;
        _clientFactory = null;
        _clientName = null;
        return this;
    }

    /// <summary>Uses a client created by <see cref="IHttpClientFactory"/>.</summary>
    /// <param name="factory">The host's configured client factory.</param>
    /// <param name="name">Optional named-client configuration.</param>
    public HttpClientRestTransportBuilder UseFactory(IHttpClientFactory factory, string? name = null)
    {
        _clientFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        _clientName = name;
        _providedClient = null;
        _handler = null;
        return this;
    }

    /// <summary>Configures a client created by this builder, before it is wrapped by the transport.</summary>
    /// <remarks>This callback is not applied to a caller-supplied or factory-created client.</remarks>
    public HttpClientRestTransportBuilder ConfigureClient(Action<HttpClient> configure)
    {
        _configureClient = configure ?? throw new ArgumentNullException(nameof(configure));
        return this;
    }

    /// <summary>Creates the transport using the selected client source.</summary>
    public HttpClientRestTransport Build()
    {
        if (_providedClient is not null)
        {
            if (_configureClient is not null)
                throw new InvalidOperationException("ConfigureClient cannot be combined with UseClient; configure the caller-owned client before supplying it.");
            return new HttpClientRestTransport(_providedClient);
        }

        if (_clientFactory is not null)
        {
            if (_configureClient is not null)
                throw new InvalidOperationException("ConfigureClient cannot be combined with UseFactory; configure factory clients through host dependency injection.");
            return new HttpClientRestTransport(
                _clientName is null ? _clientFactory.CreateClient() : _clientFactory.CreateClient(_clientName),
                ownsClient: true);
        }

        var client = _handler is null
            ? new HttpClient()
            : new HttpClient(_handler, _disposeHandler);

        try
        {
            _configureClient?.Invoke(client);
            return new HttpClientRestTransport(client, ownsClient: true);
        }
        catch
        {
            client.Dispose();
            throw;
        }
    }
}
