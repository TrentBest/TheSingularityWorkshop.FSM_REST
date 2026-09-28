namespace TheSingularityWorkshop.FSM_REST;

public sealed record RestRequest(
    string Method,
    Uri Uri,
    IReadOnlyDictionary<string, string>? Headers = null,
    string? Body = null,
    string? ContentType = null);
