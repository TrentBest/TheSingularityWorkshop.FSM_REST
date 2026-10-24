using TheSingularityWorkshop.FSM_REST;

namespace TheSingularityWorkshop.FSM_REST.GUI;

/// <summary>Resolves GUI mapping inputs, executes a described REST operation, and publishes its response.</summary>
/// <remarks>
/// This adapter accepts the neutral API descriptor and transport directly. It does not require OpenAPI,
/// a GUI framework, or a particular application-state container. The host supplies state resolution,
/// transport policy, authorization, rendering, and result publication.
/// </remarks>
public static class RestGuiOperationExecutor
{
    /// <summary>Resolves mapped values and user inputs, invokes the operation, and optionally publishes the response.</summary>
    public static async Task<RestResponse> ExecuteAsync(
        RestApiDescriptor api,
        Uri baseUri,
        IRestTransport transport,
        RestGuiMapping mapping,
        Func<string, string?> resolveSource,
        IReadOnlyDictionary<string, string?>? userInputs = null,
        Action<string, RestResponse>? publishResult = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(baseUri);
        ArgumentNullException.ThrowIfNull(transport);
        ArgumentNullException.ThrowIfNull(mapping);
        ArgumentNullException.ThrowIfNull(resolveSource);

        var operation = api.Operations.SingleOrDefault(
            candidate => string.Equals(candidate.OperationId, mapping.OperationId, StringComparison.Ordinal));
        if (operation is null)
            throw new KeyNotFoundException($"REST operation '{mapping.OperationId}' was not found in API '{api.Title}'.");

        var parameters = new Dictionary<string, string?>(StringComparer.Ordinal);
        foreach (var input in mapping.Inputs)
        {
            var value = input.SourcePath is not null
                ? resolveSource(input.SourcePath)
                : userInputs is not null && userInputs.TryGetValue(input.ParameterName, out var supplied)
                    ? supplied
                    : null;

            if (input.Required && string.IsNullOrEmpty(value))
                throw new ArgumentException($"Required GUI input '{input.ParameterName}' did not resolve to a value.");

            if (value is not null)
                parameters[input.ParameterName] = value;
        }

        var response = await RestOperationExecutor.ExecuteAsync(
            operation,
            baseUri,
            transport,
            parameters: parameters,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (mapping.ResultTarget is not null && publishResult is not null)
            publishResult(mapping.ResultTarget, response);

        return response;
    }
}
