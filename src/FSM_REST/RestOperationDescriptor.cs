namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Describes one REST operation as a composable capability.</summary>
/// <param name="Method">HTTP method, such as <c>GET</c> or <c>POST</c>.</param>
/// <param name="Path">Relative API path.</param>
/// <param name="OperationId">Stable operation identity from the description source.</param>
/// <param name="Summary">Optional concise human-readable summary.</param>
/// <param name="Description">Optional detailed description.</param>
/// <param name="Parameters">Parameters accepted by the operation.</param>
/// <param name="RequestBody">Optional request-body description.</param>
/// <param name="Responses">Optional response descriptions.</param>
public sealed record RestOperationDescriptor(
    string Method,
    string Path,
    string OperationId,
    string? Summary,
    string? Description,
    IReadOnlyList<RestParameterDescriptor> Parameters,
    RestRequestBodyDescriptor? RequestBody,
    IReadOnlyList<RestResponseDescriptor>? Responses = null)
{
    private string _method = RequireText(Method, nameof(Method));
    private string _path = RequireText(Path, nameof(Path));
    private string _operationId = RequireText(OperationId, nameof(OperationId));
    private IReadOnlyList<RestParameterDescriptor> _parameters =
        Snapshot(Parameters, nameof(Parameters));
    private IReadOnlyList<RestResponseDescriptor>? _responses =
        Responses is null ? null : Snapshot(Responses, nameof(Responses));

    /// <summary>Gets the non-empty HTTP method token.</summary>
    public string Method
    {
        get => _method;
        init => _method = RequireText(value, nameof(Method));
    }

    /// <summary>Gets the non-empty relative API path.</summary>
    public string Path
    {
        get => _path;
        init => _path = RequireText(value, nameof(Path));
    }

    /// <summary>Gets the non-empty stable operation identity.</summary>
    public string OperationId
    {
        get => _operationId;
        init => _operationId = RequireText(value, nameof(OperationId));
    }

    /// <summary>Gets a read-only snapshot of the parameters accepted by the operation.</summary>
    public IReadOnlyList<RestParameterDescriptor> Parameters
    {
        get => _parameters;
        init => _parameters = Snapshot(value, nameof(Parameters));
    }

    /// <summary>Gets a read-only snapshot of response descriptors, if supplied.</summary>
    public IReadOnlyList<RestResponseDescriptor>? Responses
    {
        get => _responses;
        init => _responses = value is null ? null : Snapshot(value, nameof(Responses));
    }

    /// <summary>Gets the label a downstream GUI or capability palette can display.</summary>
    public string PaletteLabel => string.IsNullOrWhiteSpace(Summary)
        ? $"{Method} {Path}"
        : Summary!;

    /// <summary>Gets response descriptors, or an empty collection when none were supplied.</summary>
    public IReadOnlyList<RestResponseDescriptor> ResponseDescriptors =>
        Responses ?? Array.Empty<RestResponseDescriptor>();

    private static string RequireText(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or whitespace.", paramName);
        return value;
    }

    private static IReadOnlyList<T> Snapshot<T>(IReadOnlyList<T> values, string paramName)
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        var copy = values.ToArray();
        if (copy.Any(value => value is null))
            throw new ArgumentException("Collections cannot contain null entries.", paramName);
        return Array.AsReadOnly(copy);
    }
}
