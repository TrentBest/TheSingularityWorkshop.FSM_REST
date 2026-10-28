namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Describes the body accepted by a REST operation.</summary>
/// <param name="Required">Whether the operation requires a request body.</param>
/// <param name="MediaTypes">Media types accepted by the operation.</param>
/// <param name="SchemaType">Optional logical schema type.</param>
/// <param name="SchemaFormat">Optional schema format hint.</param>
public sealed record RestRequestBodyDescriptor(
    bool Required,
    IReadOnlyList<string> MediaTypes,
    string? SchemaType,
    string? SchemaFormat)
{
    private IReadOnlyList<string> _mediaTypes = Snapshot(MediaTypes, nameof(MediaTypes));

    /// <summary>Gets a read-only snapshot of accepted media types.</summary>
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
