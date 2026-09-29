namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Describes the body accepted by a REST operation.</summary>
/// <param name="Required">Whether the operation requires a request body.</param>
/// <param name="MediaTypes">Media types accepted by the operation.</param>
/// <param name="SchemaType">Optional logical schema type.</param>
/// <param name="SchemaFormat">Optional schema format hint.</param>
public sealed record RestRequestBodyDescriptor(
    bool Required,
    IReadOnlyList<string> MediaTypes,
    string? SchemaType,
    string? SchemaFormat);
