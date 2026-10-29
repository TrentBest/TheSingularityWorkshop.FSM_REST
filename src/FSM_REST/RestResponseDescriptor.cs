namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Describes a response a REST operation may produce.</summary>
/// <param name="StatusCode">HTTP status code represented by the response.</param>
/// <param name="Description">Optional human-readable response description.</param>
/// <param name="MediaTypes">Media types represented by the response.</param>
/// <param name="SchemaType">Optional logical response schema type.</param>
/// <param name="SchemaFormat">Optional schema format hint.</param>
public sealed record RestResponseDescriptor(
    string StatusCode,
    string? Description,
    IReadOnlyList<string> MediaTypes,
    string? SchemaType,
    string? SchemaFormat)
{
    private string _statusCode = RequireText(StatusCode, nameof(StatusCode));
    private IReadOnlyList<string> _mediaTypes = Snapshot(MediaTypes, nameof(MediaTypes));

    /// <summary>Gets the non-empty HTTP status code or status-code key (for example, <c>default</c>).</summary>
    public string StatusCode
    {
        get => _statusCode;
        init => _statusCode = RequireText(value, nameof(StatusCode));
    }

    /// <summary>Gets a read-only snapshot of represented media types.</summary>
    public IReadOnlyList<string> MediaTypes
    {
        get => _mediaTypes;
        init => _mediaTypes = Snapshot(value, nameof(MediaTypes));
    }

    private static string RequireText(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or whitespace.", paramName);
        return value;
    }

    private static IReadOnlyList<string> Snapshot(IReadOnlyList<string> values, string paramName)
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        var copy = values.ToArray();
        if (copy.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Media types cannot contain null or whitespace entries.", paramName);
        return Array.AsReadOnly(copy);
    }
}
