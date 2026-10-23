namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Maps one declared REST parameter to either a GUI input or an application-state source.</summary>
/// <param name="ParameterName">Name of the parameter declared by the REST operation.</param>
/// <param name="Label">Human-readable label for a generated GUI control.</param>
/// <param name="ValueType">Optional renderer-neutral control/value hint, such as text, number, date, or choice.</param>
/// <param name="SourcePath">Optional application-state path supplying the value; null means the user provides it.</param>
/// <param name="Required">Whether the consuming GUI should treat the value as required.</param>
public sealed record RestGuiInputMapping(
    string ParameterName,
    string Label,
    string? ValueType = null,
    string? SourcePath = null,
    bool Required = false)
{
    /// <summary>Validates required identifiers and the optional source path.</summary>
    public RestGuiInputMapping Validate()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ParameterName);
        ArgumentException.ThrowIfNullOrWhiteSpace(Label);
        if (SourcePath is not null && string.IsNullOrWhiteSpace(SourcePath))
            throw new ArgumentException("A data source path must be non-empty when supplied.", nameof(SourcePath));
        return this;
    }
}
