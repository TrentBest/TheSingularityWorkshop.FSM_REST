namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Resolves a GUI mapping's inputs, executes its REST operation, and publishes its response.</summary>
/// <remarks>
/// The executor does not depend on a GUI framework or a particular application-state container.
/// The host supplies source-path resolution and result publication delegates, which keeps data
/// access, authorization, rendering, and state mutation under the host's control.
/// </remarks>
public static class RestGuiOperationExecutor
{
    /// <summary>
    /// Resolves mapped values and user inputs, invokes the operation, and optionally publishes the response.
    /// </summary>
    /// <param name="connection">Connected API used to execute the mapped operation.</param>
    /// <param name="mapping">Validated GUI/data mapping for the operation.</param>
    /// <param name="resolveSource">Resolves an application-state path to a scalar parameter value.</param>
    /// <param name="userInputs">Values collected from controls for mappings without a source path.</param>
    /// <param name="publishResult">Optional callback for publishing the response to the mapped result target.</param>
    /// <param name="cancellationToken">Cancels the operation invocation.</param>
    public static async Task<RestResponse> ExecuteAsync(
        OpenApiRestApiConnection connection,
        RestGuiMapping mapping,
        Func<string, string?> resolveSource,
        IReadOnlyDictionary<string, string?>? userInputs = null,
        Action<string, RestResponse>? publishResult = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(mapping);
        ArgumentNullException.ThrowIfNull(resolveSource);

        var parameters = new Dictionary<string, string?>(StringComparer.Ordinal);
        foreach (var input in mapping.Inputs)
        {
            string? value;
            if (input.SourcePath is not null)
            {
                value = resolveSource(input.SourcePath);
            }
            else
            {
                value = userInputs is not null && userInputs.TryGetValue(input.ParameterName, out var supplied)
                    ? supplied
                    : null;
            }

            if (input.Required && string.IsNullOrEmpty(value))
                throw new ArgumentException($"Required GUI input '{input.ParameterName}' did not resolve to a value.");

            if (value is not null)
                parameters[input.ParameterName] = value;
        }

        var response = await connection.ExecuteAsync(
            mapping.OperationId,
            parameters,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (mapping.ResultTarget is not null && publishResult is not null)
            publishResult(mapping.ResultTarget, response);

        return response;
    }
}
