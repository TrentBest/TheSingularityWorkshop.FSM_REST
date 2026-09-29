namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Adapts <see cref="HttpClient"/> to the framework-neutral <see cref="IRestTransport"/> contract.</summary>
/// <remarks>This is an adapter, not the definition of FSM_REST. Hosts can supply their own transport implementation.</remarks>
public sealed class HttpClientRestTransport : IRestTransport
{
    private readonly HttpClient _client;

    /// <summary>Creates a transport using the supplied <see cref="HttpClient"/>.</summary>
    /// <param name="client">HTTP client owned and configured by the host.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="client"/> is null.</exception>
    public HttpClientRestTransport(HttpClient client) => _client = client ?? throw new ArgumentNullException(nameof(client));

    /// <inheritdoc />
    public async Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        using var message = new HttpRequestMessage(new HttpMethod(request.Method), request.Uri);

        if (request.Headers is not null)
            foreach (var header in request.Headers)
                message.Headers.TryAddWithoutValidation(header.Key, header.Value);

        if (request.Body is not null)
            message.Content = new StringContent(request.Body, System.Text.Encoding.UTF8, request.ContentType ?? "application/json");

        using var response = await _client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var header in response.Headers)
            headers[header.Key] = string.Join(", ", header.Value);
        foreach (var header in response.Content.Headers)
            headers[header.Key] = string.Join(", ", header.Value);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        return new RestResponse((int)response.StatusCode, response.ReasonPhrase, headers, body);
    }
}
