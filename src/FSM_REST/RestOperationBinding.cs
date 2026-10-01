namespace TheSingularityWorkshop.FSM_REST;

/// <summary>
/// Represents one runtime invocation of a reusable REST operation capability.
/// </summary>
/// <remarks>
/// The operation descriptor defines the capability. This binding supplies the
/// runtime values for one invocation without mutating the descriptor.
/// </remarks>
/// <param name="Operation">Reusable operation capability being invoked.</param>
/// <param name="Parameters">Runtime values keyed by the operation's parameter names.</param>
public sealed record RestOperationBinding(
    RestOperationDescriptor Operation,
    IReadOnlyDictionary<string, string?> Parameters)
{
    public RestOperationBinding(
        RestOperationDescriptor operation,
        IReadOnlyDictionary<string, string?>? parameters = null)
        : this(
            operation ?? throw new ArgumentNullException(nameof(operation)),
            parameters is null
                ? new Dictionary<string, string?>(StringComparer.Ordinal)
                : new Dictionary<string, string?>(parameters, StringComparer.Ordinal))
    {
    }
}
