namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Describes one parameter accepted by a REST operation.</summary>
/// <param name="Name">Parameter name.</param>
/// <param name="Location">Parameter location, such as <c>query</c>, <c>path</c>, or <c>header</c>.</param>
/// <param name="Required">Whether the caller must provide the parameter.</param>
/// <param name="Type">Optional logical value type.</param>
/// <param name="Format">Optional format hint.</param>
/// <param name="Description">Optional human-readable description.</param>
public sealed record RestParameterDescriptor(
    string Name,
    string Location,
    bool Required,
    string? Type,
    string? Format,
    string? Description)
{
    private string _name = RequireText(Name, nameof(Name));
    private string _location = RequireText(Location, nameof(Location));

    /// <summary>Gets the non-empty parameter name.</summary>
    public string Name
    {
        get => _name;
        init => _name = RequireText(value, nameof(Name));
    }

    /// <summary>Gets the non-empty parameter location.</summary>
    public string Location
    {
        get => _location;
        init => _location = RequireText(value, nameof(Location));
    }

    private static string RequireText(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or whitespace.", paramName);
        return value;
    }
}
