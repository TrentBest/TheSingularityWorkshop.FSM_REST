using System.Collections.ObjectModel;

namespace TheSingularityWorkshop.FSM_REST;

/// <summary>
/// Represents one runtime invocation of a reusable REST operation capability.
/// </summary>
/// <remarks>
/// The operation descriptor defines the capability. This binding supplies the
/// runtime values for one invocation without mutating the descriptor. Input values
/// are copied and exposed through a read-only dictionary.
/// </remarks>
public sealed record RestOperationBinding
{
    public RestOperationBinding(
        RestOperationDescriptor operation,
        IReadOnlyDictionary<string, string?>? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        Operation = operation;
        var snapshot = parameters is null
            ? new Dictionary<string, string?>(StringComparer.Ordinal)
            : new Dictionary<string, string?>(parameters, StringComparer.Ordinal);
        Parameters = new ReadOnlyDictionary<string, string?>(snapshot);
    }

    /// <summary>Reusable operation capability being invoked.</summary>
    public RestOperationDescriptor Operation { get; }

    /// <summary>Read-only snapshot of runtime values keyed by declared parameter names.</summary>
    public IReadOnlyDictionary<string, string?> Parameters { get; }
}
