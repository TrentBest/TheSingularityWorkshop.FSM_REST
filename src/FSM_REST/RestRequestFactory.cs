namespace TheSingularityWorkshop.FSM_REST;

/// <summary>
/// Binds runtime parameter values to a REST operation and produces an executable request.
/// </summary>
/// <remarks>
/// This factory deliberately stops at request construction. It does not send requests,
/// authenticate callers, cache responses, or interpret response payloads.
/// </remarks>
public static class RestRequestFactory
{
    /// <summary>
    /// Creates a request from a REST operation and its runtime values.
    /// </summary>
    /// <param name="operation">Operation whose path and parameter metadata define the request.</param>
    /// <param name="baseUri">Absolute base URI of the remote API.</param>
    /// <param name="parameters">Runtime values keyed by the parameter names declared by the operation.</param>
    /// <param name="headers">Additional request headers.</param>
    /// <param name="body">Optional request body.</param>
    /// <param name="contentType">Optional request content type.</param>
    /// <returns>An executable <see cref="RestRequest"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="operation"/> or <paramref name="baseUri"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when a required parameter is missing or an unsupported parameter location is declared.</exception>
    public static RestRequest Create(
        RestOperationDescriptor operation,
        Uri baseUri,
        IReadOnlyDictionary<string, string?>? parameters = null,
        IReadOnlyDictionary<string, string>? headers = null,
        string? body = null,
        string? contentType = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return Create(
            new RestOperationBinding(operation, parameters),
            baseUri,
            headers,
            body,
            contentType);
    }

    /// <summary>
    /// Creates a request from a runtime operation binding.
    /// </summary>
    /// <param name="binding">Reusable operation plus runtime parameter values.</param>
    /// <param name="baseUri">Absolute base URI of the remote API.</param>
    /// <param name="headers">Additional request headers.</param>
    /// <param name="body">Optional request body.</param>
    /// <param name="contentType">Optional request content type.</param>
    /// <returns>An executable request for one operation invocation.</returns>
    public static RestRequest Create(
        RestOperationBinding binding,
        Uri baseUri,
        IReadOnlyDictionary<string, string>? headers = null,
        string? body = null,
        string? contentType = null)
    {
        ArgumentNullException.ThrowIfNull(binding);
        var operation = binding.Operation;

        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(baseUri);

        if (!baseUri.IsAbsoluteUri)
            throw new ArgumentException("The REST base URI must be absolute.", nameof(baseUri));

        var values = binding.Parameters;
        var requestHeaders = headers is null
            ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string>(headers, StringComparer.OrdinalIgnoreCase);

        var path = operation.Path;
        var query = new List<string>();
        var cookies = new List<string>();

        foreach (var parameter in operation.Parameters)
        {
            values.TryGetValue(parameter.Name, out var value);

            if (string.IsNullOrEmpty(value))
            {
                if (parameter.Required)
                    throw new ArgumentException(
                        $"Required REST parameter '{parameter.Name}' was not supplied.",
                        nameof(binding));

                continue;
            }

            switch (parameter.Location.ToLowerInvariant())
            {
                case "path":
                    path = path.Replace(
                        "{" + parameter.Name + "}",
                        Uri.EscapeDataString(value),
                        StringComparison.Ordinal);
                    break;

                case "query":
                    query.Add($"{Uri.EscapeDataString(parameter.Name)}={Uri.EscapeDataString(value)}");
                    break;

                case "header":
                    requestHeaders[parameter.Name] = value;
                    break;

                case "cookie":
                    cookies.Add($"{parameter.Name}={Uri.EscapeDataString(value)}");
                    break;

                default:
                    throw new ArgumentException(
                        $"Unsupported REST parameter location '{parameter.Location}'.",
                        nameof(operation));
            }
        }

        foreach (var placeholder in operation.Parameters.Where(x =>
                     string.Equals(x.Location, "path", StringComparison.OrdinalIgnoreCase)))
        {
            if (path.Contains("{" + placeholder.Name + "}", StringComparison.Ordinal))
                throw new ArgumentException(
                    $"Path parameter '{placeholder.Name}' was not bound.",
                    nameof(parameters));
        }

        if (cookies.Count > 0)
            requestHeaders["Cookie"] = string.Join("; ", cookies);

        var relativePath = path.TrimStart('/');
        if (!baseUri.AbsolutePath.EndsWith("/", StringComparison.Ordinal))
            baseUri = new Uri(baseUri.AbsoluteUri + "/", UriKind.Absolute);

        var uri = new Uri(baseUri, relativePath);
        if (query.Count > 0)
        {
            var separator = string.IsNullOrEmpty(uri.Query) ? "?" : "&";
            uri = new Uri(uri + separator + string.Join("&", query), UriKind.Absolute);
        }

        return new RestRequest(
            operation.Method,
            uri,
            requestHeaders,
            body,
            contentType);
    }
}
