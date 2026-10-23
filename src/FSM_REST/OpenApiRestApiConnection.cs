using System.Text.Json;

namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Connects to an API described by a remote OpenAPI 3.x JSON document.</summary>
/// <remarks>
/// The connection downloads and imports the description, chooses the first declared server
/// (or the document origin when no server is declared), and makes imported operations callable.
/// The caller owns the supplied HttpClient and any authentication policy configured on it.
/// </remarks>
public sealed class OpenApiRestApiConnection
{
    private readonly IRestTransport _transport;

    private OpenApiRestApiConnection(
        Uri definitionUri,
        Uri baseUri,
        OpenApiImportResult import,
        IRestTransport transport)
    {
        DefinitionUri = definitionUri;
        BaseUri = baseUri;
        Api = import.Api;
        Warnings = import.Warnings;
        _transport = transport;
    }

    /// <summary>The URL from which the OpenAPI document was loaded.</summary>
    public Uri DefinitionUri { get; }

    /// <summary>The selected server URL used for operation calls.</summary>
    public Uri BaseUri { get; }

    /// <summary>The imported API and its operation catalogue.</summary>
    public RestApiDescriptor Api { get; }

    /// <summary>Import fidelity warnings, including unsupported or unresolved OpenAPI constructs.</summary>
    public IReadOnlyList<string> Warnings { get; }

    /// <summary>Downloads an OpenAPI 3.x JSON document and creates a callable API connection.</summary>
    /// <param name="definitionUri">Absolute URL to the Swagger/OpenAPI JSON document.</param>
    /// <param name="client">HTTP client used to retrieve the definition and send API requests.</param>
    /// <param name="cancellationToken">Cancellation for definition retrieval.</param>
    public static async Task<OpenApiRestApiConnection> ConnectAsync(
        Uri definitionUri,
        HttpClient client,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(definitionUri);
        ArgumentNullException.ThrowIfNull(client);
        if (!definitionUri.IsAbsoluteUri || (definitionUri.Scheme != Uri.UriSchemeHttp && definitionUri.Scheme != Uri.UriSchemeHttps))
            throw new ArgumentException("The OpenAPI definition URL must be an absolute HTTP or HTTPS URL.", nameof(definitionUri));

        using var response = await client.GetAsync(definitionUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var import = OpenApiJsonImporter.Import(json);
        var baseUri = ResolveServerUri(json, definitionUri);
        return new OpenApiRestApiConnection(definitionUri, baseUri, import, new HttpClientRestTransport(client));
    }

    /// <summary>Executes an imported operation by its OpenAPI operationId.</summary>
    public Task<RestResponse> ExecuteAsync(
        string operationId,
        IReadOnlyDictionary<string, string?>? parameters = null,
        IReadOnlyDictionary<string, string>? headers = null,
        string? body = null,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(operationId);
        var matches = Api.Operations.Where(operation =>
            string.Equals(operation.OperationId, operationId, StringComparison.Ordinal)).ToArray();
        if (matches.Length == 0)
            throw new KeyNotFoundException($"No REST operation with operationId '{operationId}' was imported.");
        if (matches.Length > 1)
            throw new InvalidOperationException($"The imported API contains more than one operationId '{operationId}'.");
        return RestOperationExecutor.ExecuteAsync(matches[0], BaseUri, _transport, parameters, headers, body, contentType, cancellationToken);
    }

    private static Uri ResolveServerUri(string json, Uri definitionUri)
    {
        using var document = JsonDocument.Parse(json);
        if (document.RootElement.TryGetProperty("servers", out var servers) &&
            servers.ValueKind == JsonValueKind.Array && servers.GetArrayLength() > 0)
        {
            foreach (var server in servers.EnumerateArray())
            {
                if (!server.TryGetProperty("url", out var urlElement) || urlElement.ValueKind != JsonValueKind.String)
                    continue;
                var value = urlElement.GetString();
                if (string.IsNullOrWhiteSpace(value) || value.Contains('{') || value.Contains('}'))
                    throw new ArgumentException("The first OpenAPI server URL must be concrete; server-variable substitution is not supported.");
                var origin = new Uri(definitionUri.GetLeftPart(UriPartial.Authority) + "/", UriKind.Absolute);
                Uri resolved;
                if (value.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                    value.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    resolved = new Uri(value, UriKind.Absolute);
                }
                else if (Uri.TryCreate(value, UriKind.Absolute, out var nonHttpAbsolute) &&
                         !string.IsNullOrEmpty(nonHttpAbsolute.Scheme) &&
                         value.Contains("://", StringComparison.Ordinal))
                {
                    throw new ArgumentException("The OpenAPI server URL must use HTTP or HTTPS.");
                }
                else
                {
                    resolved = new Uri(origin, value);
                }
                if (resolved.Scheme != Uri.UriSchemeHttp && resolved.Scheme != Uri.UriSchemeHttps)
                    throw new ArgumentException("The OpenAPI server URL must use HTTP or HTTPS.");
                if (!string.IsNullOrEmpty(resolved.Query) || !string.IsNullOrEmpty(resolved.Fragment))
                    throw new ArgumentException("The OpenAPI server URL must not contain a query or fragment.");
                return resolved;
            }
        }

        return new Uri(definitionUri.GetLeftPart(UriPartial.Authority) + "/", UriKind.Absolute);
    }
}
