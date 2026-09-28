namespace TheSingularityWorkshop.FSM_REST;

public sealed record RestResponseDescriptor(
    string StatusCode,
    string? Description,
    IReadOnlyList<string> MediaTypes,
    string? SchemaType,
    string? SchemaFormat);
