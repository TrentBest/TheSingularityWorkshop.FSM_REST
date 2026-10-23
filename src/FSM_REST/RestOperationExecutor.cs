namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Builds and executes one described REST operation through an injected transport.</summary>
/// <remarks>
/// This is the execution bridge between imported or hand-authored API metadata and the transport.
/// It returns the raw response; authentication, response deserialization, retries, and domain-model
/// construction remain explicit host or provider responsibilities.
/// </remarks>
public static class RestOperationExecutor
{
    /// <summary>Creates a request from an operation descriptor and sends it through the supplied transport.</summary>
    public static Task<RestResponse> ExecuteAsync(
        RestOperationDescriptor operation,
        Uri baseUri,
        IRestTransport transport,
        IReadOnlyDictionary<string, string?>? parameters = null,
        IReadOnlyDictionary<string, string>? headers = null,
        string? body = null,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(transport);

        var request = RestRequestFactory.Create(
            operation,
            baseUri,
            parameters,
            headers,
            body,
            contentType);

        return transport.SendAsync(request, cancellationToken);
    }
}
