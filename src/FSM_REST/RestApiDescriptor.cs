namespace TheSingularityWorkshop.FSM_REST;

public sealed record RestApiDescriptor(
    string Title,
    string Version,
    IReadOnlyList<RestOperationDescriptor> Operations,
    string? Description = null)
{
    public int OperationCount => Operations.Count;
}
