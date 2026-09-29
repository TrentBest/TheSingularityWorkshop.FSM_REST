namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Represents an executable request without coupling the caller to a transport implementation.</summary>
/// <remarks><see cref="IRestTransport"/> sends the request; this type does not own connection lifetime or retry policy.</remarks>
/// <param name="Method">HTTP method.</param>
/// <param name="Uri">Target URI.</param>
/// <param name="Headers">Optional request headers.</param>
/// <param name="Body">Optional request body.</param>
/// <param name="ContentType">Optional content type; the default adapter uses JSON when a body exists and no type is supplied.</param>
public sealed record RestRequest(
    string Method,
    Uri Uri,
    IReadOnlyDictionary<string, string>? Headers = null,
    string? Body = null,
    string? ContentType = null);
