namespace TheSingularityWorkshop.FSM_REST;

public sealed record RestParameterDescriptor(
    string Name,
    string Location,
    bool Required,
    string? Type,
    string? Format,
    string? Description);
