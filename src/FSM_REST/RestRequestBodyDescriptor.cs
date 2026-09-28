namespace TheSingularityWorkshop.FSM_REST;

public sealed record RestRequestBodyDescriptor(
    bool Required,
    IReadOnlyList<string> MediaTypes,
    string? SchemaType,
    string? SchemaFormat);
