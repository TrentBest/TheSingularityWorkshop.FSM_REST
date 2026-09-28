namespace TheSingularityWorkshop.FSM_REST;

public sealed class HttpClientRestTransport : IRestTransport
{
    private readonly HttpClient _client;

    public HttpClientRestTransport(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<RestResponse> SendAsync(
        RestRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var message = new HttpRequestMessage(
            new HttpMethod(request.Method),
            request.Uri);

        if (request.Headers is not null)
        {
            foreach (var header in request.Headers)
                message.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (request.Body is not null)
        {
            message.Content = new StringContent(
                request.Body,
                System.Text.Encoding.UTF8,
                request.ContentType ?? "application/json");
        }

        using var response = await _client.SendAsync(
            message,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var header in response.Headers)
            headers[header.Key] = string.Join(", ", header.Value);

        foreach (var header in response.Content.Headers)
            headers[header.Key] = string.Join(", ", header.Value);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        return new RestResponse(
            (int)response.StatusCode,
            response.ReasonPhrase,
            headers,
            body);
    }
}
