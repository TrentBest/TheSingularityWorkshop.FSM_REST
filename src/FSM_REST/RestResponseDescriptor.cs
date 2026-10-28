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
    private IReadOnlyList<string> _mediaTypes = Snapshot(MediaTypes, nameof(MediaTypes));

    /// <summary>Gets a read-only snapshot of represented media types.</summary>
    public IReadOnlyList<string> MediaTypes
    {
        get => _mediaTypes;
        init => _mediaTypes = Snapshot(value, nameof(MediaTypes));
    }

    private static IReadOnlyList<string> Snapshot(IReadOnlyList<string> values, string paramName)
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        var copy = values.ToArray();
        if (copy.Any(value => value is null))
            throw new ArgumentException("Collections cannot contain null entries.", paramName);
        return Array.AsReadOnly(copy);
    }
}
