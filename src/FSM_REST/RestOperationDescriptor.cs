namespace TheSingularityWorkshop.FSM_REST;

public sealed record RestOperationDescriptor(
    string Method,
    string Path,
    string OperationId,
    string? Summary,
    string? Description,
    IReadOnlyList<RestParameterDescriptor> Parameters,
    RestRequestBodyDescriptor? RequestBody,
    IReadOnlyList<RestResponseDescriptor>? Responses = null)
{
    public string PaletteLabel => string.IsNullOrWhiteSpace(Summary)
        ? $"{Method} {Path}"
        : Summary!;

    public IReadOnlyList<RestResponseDescriptor> ResponseDescriptors =>
        Responses ?? Array.Empty<RestResponseDescriptor>();
}
