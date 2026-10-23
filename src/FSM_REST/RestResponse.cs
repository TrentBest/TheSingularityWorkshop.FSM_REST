using System.Text.Json;

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
    private static readonly JsonSerializerOptions DefaultJsonOptions = new(JsonSerializerDefaults.Web);

    /// <summary>Gets whether the response status is in the HTTP 2xx success range.</summary>
    public bool IsSuccessStatusCode => StatusCode is >= 200 and < 300;

    /// <summary>Deserializes a JSON response body into an application-owned type.</summary>
    /// <typeparam name="T">Target type chosen by the consuming application.</typeparam>
    /// <param name="options">Optional JSON serializer settings.</param>
    /// <returns>The deserialized value, or the JSON null value when the body is JSON null.</returns>
    /// <remarks>This performs JSON deserialization only; it does not validate the response against the API schema.</remarks>
    public T? Deserialize<T>(JsonSerializerOptions? options = null) =>
        JsonSerializer.Deserialize<T>(Body, options ?? DefaultJsonOptions);
}
