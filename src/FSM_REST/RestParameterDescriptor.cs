namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Describes one parameter accepted by a REST operation.</summary>
/// <param name="Name">Parameter name.</param>
/// <param name="Location">Parameter location, such as <c>query</c>, <c>path</c>, or <c>header</c>.</param>
/// <param name="Required">Whether the caller must provide the parameter.</param>
/// <param name="Type">Optional logical value type.</param>
/// <param name="Format">Optional format hint.</param>
/// <param name="Description">Optional human-readable description.</param>
public sealed record RestParameterDescriptor(
    string Name,
    string Location,
    bool Required,
    string? Type,
    string? Format,
    string? Description);
