namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Represents an observed transport response without interpreting its domain payload.</summary>
/// <param name="StatusCode">HTTP status code.</param>
/// <param name="ReasonPhrase">Optional HTTP reason phrase.</param>
/// <param name="Headers">Response and content headers.</param>
/// <param name="Body">Response body as received by the transport.</param>
public sealed record RestResponse(
    int StatusCode,
    string? ReasonPhrase,
    IReadOnlyDictionary<string, string> Headers,
    string Body)
{
    /// <summary>Gets whether the response status is in the HTTP 2xx success range.</summary>
    public bool IsSuccessStatusCode => StatusCode is >= 200 and < 300;
}
