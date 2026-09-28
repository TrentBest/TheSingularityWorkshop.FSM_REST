namespace TheSingularityWorkshop.FSM_REST;

public sealed record RestOperationDescriptor(
    string Method,
    string Path,
    string OperationId,
    string? Summary,
    string? Description,
    IReadOnlyList<RestParameterDescriptor> Parameters,
    RestRequestBodyDescriptor? RequestBody)
{
    public string PaletteLabel => string.IsNullOrWhiteSpace(Summary)
        ? $"{Method} {Path}"
        : Summary!;
}
