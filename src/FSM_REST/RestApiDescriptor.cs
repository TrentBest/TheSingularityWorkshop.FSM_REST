namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Describes a REST API as a protocol-neutral collection of operations.</summary>
/// <remarks>
/// This type describes capability; it does not discover, execute, authenticate,
/// render, or host the API. Description-format adapters translate their own models
/// into this neutral representation. Collections are snapshotted at assignment so
/// later changes to caller-owned collections cannot mutate the descriptor.
/// </remarks>
/// <param name="Title">Human-readable API title.</param>
/// <param name="Version">Version supplied by the capability owner.</param>
/// <param name="Operations">Operations exposed by the API.</param>
/// <param name="Description">Optional API description.</param>
public sealed record RestApiDescriptor(
    string Title,
    string Version,
    IReadOnlyList<RestOperationDescriptor> Operations,
    string? Description = null)
{
    private IReadOnlyList<RestOperationDescriptor> _operations =
        Snapshot(Operations, nameof(Operations));

    /// <summary>Gets a read-only snapshot of the operations exposed by the API.</summary>
    public IReadOnlyList<RestOperationDescriptor> Operations
    {
        get => _operations;
        init => _operations = Snapshot(value, nameof(Operations));
    }

    /// <summary>Gets the number of operations described by the API.</summary>
    public int OperationCount => Operations.Count;

    private static IReadOnlyList<T> Snapshot<T>(IReadOnlyList<T> values, string paramName)
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        var copy = values.ToArray();
        if (copy.Any(value => value is null))
            throw new ArgumentException("Collections cannot contain null entries.", paramName);
        return Array.AsReadOnly(copy);
    }
}
