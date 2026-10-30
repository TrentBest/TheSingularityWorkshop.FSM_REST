using System.Collections.ObjectModel;

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
        var headerValues = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        CaptureHeaders(response.Headers, headers, headerValues);
        CaptureHeaders(response.Content.Headers, headers, headerValues);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        var immutableHeaderValues = new ReadOnlyDictionary<string, IReadOnlyList<string>>(
            headerValues.ToDictionary(
                pair => pair.Key,
                pair => (IReadOnlyList<string>)Array.AsReadOnly(pair.Value.ToArray()),
                StringComparer.OrdinalIgnoreCase));

        return new RestResponse((int)response.StatusCode, response.ReasonPhrase, headers, body)
        {
            HeaderValues = immutableHeaderValues
        };
    }

    private static void CaptureHeaders(
        System.Net.Http.Headers.HttpHeaders source,
        IDictionary<string, string> flattened,
        IDictionary<string, List<string>> values)
    {
        foreach (var header in source)
        {
            var headerValues = header.Value.ToArray();
            flattened[header.Key] = string.Join(", ", headerValues);

            if (!values.TryGetValue(header.Key, out var collected))
            {
                collected = new List<string>();
                values.Add(header.Key, collected);
            }

            collected.AddRange(headerValues);
        }
    }
}
