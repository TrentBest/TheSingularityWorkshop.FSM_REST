namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Describes a REST API as a protocol-neutral collection of operations.</summary>
/// <remarks>
/// This type describes capability; it does not discover, execute, authenticate,
/// render, or host the API. Description-format adapters translate their own models
/// into this neutral representation.
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
    /// <summary>Gets the number of operations described by the API.</summary>
    public int OperationCount => Operations.Count;
}
