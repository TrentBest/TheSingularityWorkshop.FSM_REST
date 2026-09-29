namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Describes a response a REST operation may produce.</summary>
/// <param name="StatusCode">HTTP status code represented by the response.</param>
/// <param name="Description">Optional human-readable response description.</param>
/// <param name="MediaTypes">Media types represented by the response.</param>
/// <param name="SchemaType">Optional logical response schema type.</param>
/// <param name="SchemaFormat">Optional schema format hint.</param>
public sealed record RestResponseDescriptor(
    string StatusCode,
    string? Description,
    IReadOnlyList<string> MediaTypes,
    string? SchemaType,
    string? SchemaFormat);
