namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Builds an <see cref="HttpClientRestTransport"/> with explicit client ownership.</summary>
/// <remarks>
/// This builder configures a client it creates or accepts a caller-owned client without mutating it.
/// A built transport should be disposed when its lifetime ends; caller-owned clients are never disposed by the transport. Each builder is single-use.
/// </remarks>
public sealed class HttpClientRestTransportBuilder
{
    private HttpClient? _providedClient;
    private HttpMessageHandler? _handler;
    private bool _disposeHandler = true;
    private Action<HttpClient>? _configureClient;
    private bool _built;
    private bool _clientSourceSelected;

    /// <summary>Creates a builder that will create and own a new <see cref="HttpClient"/>.</summary>
    public static HttpClientRestTransportBuilder Create() => new();

    /// <summary>Uses a client supplied and owned by the caller.</summary>
    public HttpClientRestTransportBuilder UseClient(HttpClient client)
    {
        EnsureNotBuilt();
        EnsureClientSourceNotSelected();
        _clientSourceSelected = true;
        _providedClient = client ?? throw new ArgumentNullException(nameof(client));
        _handler = null;
        return this;
    }

    /// <summary>Creates a client around a caller-supplied handler.</summary>
    /// <param name="handler">Handler used by the new client.</param>
    /// <param name="disposeHandler">Whether disposing the built transport also disposes the handler.</param>
    public HttpClientRestTransportBuilder UseHandler(HttpMessageHandler handler, bool disposeHandler = true)
    {
        EnsureNotBuilt();
        EnsureClientSourceNotSelected();
        _clientSourceSelected = true;
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        _disposeHandler = disposeHandler;
        _providedClient = null;
        return this;
    }

    /// <summary>Configures a client created by this builder, before it is wrapped by the transport.</summary>
    /// <remarks>This callback is not applied to a caller-supplied client.</remarks>
    public HttpClientRestTransportBuilder ConfigureClient(Action<HttpClient> configure)
    {
        EnsureNotBuilt();
        _configureClient = configure ?? throw new ArgumentNullException(nameof(configure));
        return this;
    }

    /// <summary>Creates the transport using the selected client source. This builder can only build once.</summary>
    public HttpClientRestTransport Build()
    {
        EnsureNotBuilt();

        if (_providedClient is not null && _configureClient is not null)
            throw new InvalidOperationException("ConfigureClient cannot be combined with UseClient; configure the caller-owned client before supplying it.");

        // A handler can only be attached to one HttpClient. Mark the builder consumed before creating resources.
        _built = true;

        if (_providedClient is not null)
            return new HttpClientRestTransport(_providedClient);

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

    private void EnsureClientSourceNotSelected()
    {
        if (_clientSourceSelected)
            throw new InvalidOperationException("A client source has already been selected. Create a new builder to choose another source.");
    }

    private void EnsureNotBuilt()
    {
        if (_built)
            throw new InvalidOperationException("This builder has already built a transport. Create a new builder for another transport.");
    }
}
