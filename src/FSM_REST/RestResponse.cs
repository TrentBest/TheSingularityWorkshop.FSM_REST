using System.Collections.ObjectModel;
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

    private static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> EmptyHeaderValues =
        new ReadOnlyDictionary<string, IReadOnlyList<string>>(
            new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase));

    private IReadOnlyDictionary<string, IReadOnlyList<string>> _headerValues = EmptyHeaderValues;

    /// <summary>
    /// Gets response and content headers with each field value preserved separately.
    /// Header names are compared case-insensitively. Unlike <see cref="Headers"/>,
    /// this representation does not combine repeated values with commas.
    /// </summary>
    /// <remarks>
    /// This is especially important for headers such as <c>Set-Cookie</c>, whose
    /// repeated values cannot safely be reconstructed from a comma-joined string.
    /// Values assigned during construction are copied into a case-insensitive,
    /// read-only snapshot so callers cannot mutate the response through their source collections.
    /// Responses constructed directly retain an empty collection unless values are supplied.
    /// </remarks>
    public IReadOnlyDictionary<string, IReadOnlyList<string>> HeaderValues
    {
        get => _headerValues;
        init => _headerValues = SnapshotHeaderValues(value);
    }

    /// <summary>Gets whether the response status is in the HTTP 2xx success range.</summary>
    public bool IsSuccessStatusCode => StatusCode is >= 200 and < 300;

    /// <summary>Deserializes a JSON response body into an application-owned type.</summary>
    /// <typeparam name="T">Target type chosen by the consuming application.</typeparam>
    /// <param name="options">Optional JSON serializer settings.</param>
    /// <returns>The deserialized value, or the JSON null value when the body is JSON null.</returns>
    /// <remarks>This performs JSON deserialization only; it does not validate the response against the API schema.</remarks>
    public T? Deserialize<T>(JsonSerializerOptions? options = null) =>
        JsonSerializer.Deserialize<T>(Body, options ?? DefaultJsonOptions);

    private static IReadOnlyDictionary<string, IReadOnlyList<string>> SnapshotHeaderValues(
        IReadOnlyDictionary<string, IReadOnlyList<string>> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        var snapshot = new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in values)
        {
            ArgumentNullException.ThrowIfNull(pair.Value);
            if (!snapshot.TryAdd(pair.Key, Array.AsReadOnly(pair.Value.ToArray())))
                throw new ArgumentException(
                    $"Duplicate response header name '{pair.Key}' (header names are case-insensitive).",
                    nameof(values));
        }

        return new ReadOnlyDictionary<string, IReadOnlyList<string>>(snapshot);
    }
}
