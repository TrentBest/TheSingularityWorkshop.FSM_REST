namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Adapts <see cref="HttpClient"/> to the framework-neutral <see cref="IRestTransport"/> contract.</summary>
/// <remarks>This is an adapter, not the definition of FSM_REST. Hosts can supply their own transport implementation.</remarks>
public sealed class HttpClientRestTransport : IRestTransport, IDisposable
{
    private readonly HttpClient _client;
    private readonly bool _ownsClient;
    private bool _disposed;

    /// <summary>Creates a transport using the supplied <see cref="HttpClient"/> without taking ownership of it.</summary>
    /// <param name="client">HTTP client owned and configured by the host.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="client"/> is null.</exception>
    public HttpClientRestTransport(HttpClient client) : this(client, ownsClient: false) { }

    internal HttpClientRestTransport(HttpClient client, bool ownsClient)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _ownsClient = ownsClient;
    }

    /// <summary>Sends a request and captures the response as text.</summary>
    /// <remarks>Although headers are read before the response body, this API materializes the full body as a string and is not a streaming API.</remarks>
    public async Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        using var message = new HttpRequestMessage(new HttpMethod(request.Method), request.Uri);

        if (request.Body is not null)
            message.Content = new StringContent(request.Body, System.Text.Encoding.UTF8, request.ContentType ?? "application/json");

        if (request.Headers is not null)
        {
            foreach (var header in request.Headers)
            {
                if (message.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    continue;

                // HttpClient separates request headers from entity/content headers.
                // Do not silently drop a valid content header just because it was supplied
                // through the transport-neutral request header collection.
                if (message.Content is not null
                    && message.Content.Headers.TryAddWithoutValidation(header.Key, header.Value))
                    continue;

                throw new ArgumentException(
                    $"Request header '{header.Key}' could not be applied to the HTTP request.",
                    nameof(request));
            }
        }

        using var response = await _client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var header in response.Headers)
            AddResponseHeader(headers, header.Key, header.Value);
        foreach (var header in response.Content.Headers)
            AddResponseHeader(headers, header.Key, header.Value);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        return new RestResponse((int)response.StatusCode, response.ReasonPhrase, headers, body);
    }


    private static void AddResponseHeader(
        IDictionary<string, string> headers,
        string name,
        IEnumerable<string> values)
    {
        var value = string.Join(", ", values);
        if (headers.TryGetValue(name, out var existing))
            headers[name] = string.IsNullOrEmpty(existing) ? value : $"{existing}, {value}";
        else
            headers[name] = value;
    }

    /// <summary>Disposes the client only when this transport was created with ownership of that client.</summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        if (_ownsClient)
            _client.Dispose();
    }
}
