using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.FSM_REST;
using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_REST.COS;

/// <summary>Carries a REST API capability as an ecosystem MicroBundle for FSM_COS.</summary>
/// <remarks>
/// This is an integration adapter, not part of the REST transport boundary.
/// The REST package describes and transports REST capabilities; this package
/// composes those capabilities into FSM_COS.
/// </remarks>
public sealed class RestMicroBundle : IMicroBundle
{
    /// <summary>Creates a REST MicroBundle from a REST API descriptor.</summary>
    public RestMicroBundle(
        ulong id,
        RestApiDescriptor api,
        string? version = null,
        IEnumerable<MicroBundleDependency>? dependencies = null,
        IEnumerable<MicroBundleProvider>? providers = null)
    {
        ArgumentNullException.ThrowIfNull(api);

        Descriptor = new MicroBundleDescriptor(
            id,
            version ?? api.Version,
            dependencies,
            providers ?? [new MicroBundleProvider($"rest:{api.Title}")]);

        Api = api;
    }

    /// <summary>REST capability carried by this bundle.</summary>
    public RestApiDescriptor Api { get; }

    /// <inheritdoc />
    public ulong Id => Descriptor.Id;

    /// <inheritdoc />
    public MicroBundleDescriptor Descriptor { get; }

    /// <inheritdoc />
    public IReadOnlyList<BundleRequest> Dependencies =>
        Descriptor.Dependencies
            .Select(dependency => BundleRequest.Unconfigured(dependency.BundleId))
            .ToArray();

    /// <inheritdoc />
    public void Load(MicroBundleLoadContext context) =>
        ArgumentNullException.ThrowIfNull(context);

    /// <inheritdoc />
    public bool Arbitrate(ArbitrationContext context, int roundIndex)
    {
        ArgumentNullException.ThrowIfNull(context);
        return false;
    }
}
