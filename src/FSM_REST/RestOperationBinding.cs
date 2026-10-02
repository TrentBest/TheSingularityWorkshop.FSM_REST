namespace TheSingularityWorkshop.FSM_REST;

/// <summary>
/// Represents one runtime invocation of a reusable REST operation capability.
/// </summary>
/// <remarks>
/// The operation descriptor defines the capability. This binding supplies the
/// runtime values for one invocation without mutating the descriptor.
/// </remarks>
public sealed record RestOperationBinding
{
    /// <summary>Creates a runtime binding for one REST operation invocation.</summary>
    public RestOperationBinding(
        RestOperationDescriptor operation,
        IReadOnlyDictionary<string, string?>? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        Operation = operation;
        Parameters = parameters is null
            ? new Dictionary<string, string?>(StringComparer.Ordinal)
            : new Dictionary<string, string?>(parameters, StringComparer.Ordinal);
    }

    /// <summary>Reusable operation capability being invoked.</summary>
    public RestOperationDescriptor Operation { get; }

    /// <summary>Runtime values keyed by the operation's parameter names.</summary>
    public IReadOnlyDictionary<string, string?> Parameters { get; }
}
