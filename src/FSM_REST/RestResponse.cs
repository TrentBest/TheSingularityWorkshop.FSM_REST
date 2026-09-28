namespace TheSingularityWorkshop.FSM_REST;

public sealed record RestResponse(
    int StatusCode,
    string? ReasonPhrase,
    IReadOnlyDictionary<string, string> Headers,
    string Body)
{
    public bool IsSuccessStatusCode => StatusCode is >= 200 and < 300;
}
