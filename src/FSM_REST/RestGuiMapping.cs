namespace TheSingularityWorkshop.FSM_REST;

/// <summary>
/// Describes how one REST operation should be exposed by a consuming GUI and where its
/// runtime inputs and result belong in application state.
/// </summary>
/// <remarks>
/// This is renderer-neutral mapping metadata. A GUI adapter turns the input descriptors
/// into controls and uses the source/target paths to connect application data. It does not
/// create GUI objects, evaluate paths, or grant permission to invoke an operation.
/// </remarks>
public sealed record RestGuiMapping
{
    public RestGuiMapping(
        RestOperationDescriptor operation,
        string label,
        string presentation,
        IEnumerable<RestGuiInputMapping>? inputs = null,
        string? resultTarget = null,
        bool showLoadingState = true,
        bool showErrorState = true)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        ArgumentException.ThrowIfNullOrWhiteSpace(presentation);

        OperationId = operation.OperationId;
        Label = label;
        Presentation = presentation;
        ResultTarget = NormalizeOptionalPath(resultTarget, nameof(resultTarget));
        ShowLoadingState = showLoadingState;
        ShowErrorState = showErrorState;

        var declaredParameters = operation.Parameters
            .Select(parameter => parameter.Name)
            .ToHashSet(StringComparer.Ordinal);
        var mappedInputs = (inputs ?? []).Select(input => input.Validate()).ToArray();
        var duplicate = mappedInputs.GroupBy(input => input.ParameterName, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
            throw new ArgumentException($"Input parameter '{duplicate.Key}' is mapped more than once.", nameof(inputs));

        foreach (var input in mappedInputs)
        {
            if (!declaredParameters.Contains(input.ParameterName))
                throw new ArgumentException($"Input '{input.ParameterName}' is not declared by operation '{operation.OperationId}'.", nameof(inputs));
        }

        Inputs = Array.AsReadOnly(mappedInputs);
    }

    /// <summary>Stable operation identity to invoke when the mapped GUI action is triggered.</summary>
    public string OperationId { get; }

    /// <summary>Human-readable label for the GUI action or panel.</summary>
    public string Label { get; }

    /// <summary>Renderer-neutral presentation hint, such as button, form, table, or detail.</summary>
    public string Presentation { get; }

    /// <summary>Input controls and/or application-state source paths mapped to operation parameters.</summary>
    public IReadOnlyList<RestGuiInputMapping> Inputs { get; }

    /// <summary>Optional application-state path that receives the operation response.</summary>
    public string? ResultTarget { get; }

    /// <summary>Whether the consuming GUI should expose a pending/loading state during execution.</summary>
    public bool ShowLoadingState { get; }

    /// <summary>Whether the consuming GUI should expose invocation failures.</summary>
    public bool ShowErrorState { get; }

    private static string? NormalizeOptionalPath(string? path, string parameterName)
    {
        if (path is null)
            return null;
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("A data path must be non-empty when supplied.", parameterName);
        return path;
    }
}
