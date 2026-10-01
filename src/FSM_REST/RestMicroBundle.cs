using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_REST;

/// <summary>
/// Carries a REST API capability as an ecosystem MicroBundle.
/// </summary>
/// <remarks>
/// This adapter intentionally carries the REST capability into FSM_COS without
/// making FSM_REST responsible for hosting, transport policy, or response interpretation.
/// The bundle is the capability recipe; remote responses remain runtime data.
/// </remarks>
public sealed class RestMicroBundle : IMicroBundle
{
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
    public MicroBundleDescriptor Descriptor { get; }

    /// <inheritdoc />
    public IReadOnlyList<BundleRequest> Dependencies =>
        Descriptor.Dependencies
            .Select(dependency => BundleRequest.Unconfigured(dependency.BundleId))
            .ToArray();

    /// <inheritdoc />
    public void Load(MicroBundleLoadContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    /// <inheritdoc />
    public bool Arbitrate(ArbitrationContext context, int roundIndex)
    {
        ArgumentNullException.ThrowIfNull(context);
        return false;
    }
}
