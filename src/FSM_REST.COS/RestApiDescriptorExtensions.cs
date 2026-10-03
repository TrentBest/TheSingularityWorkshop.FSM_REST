using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_REST.COS;

/// <summary>Provides FSM_COS composition helpers for REST API descriptors.</summary>
public static class RestApiDescriptorExtensions
{
    /// <summary>
    /// Creates a MicroBundle carrying the REST API capability.
    /// </summary>
    public static RestMicroBundle ToMicroBundle(
        this TheSingularityWorkshop.FSM_REST.RestApiDescriptor api,
        ulong id,
        string? version = null,
        IEnumerable<MicroBundleDependency>? dependencies = null,
        IEnumerable<MicroBundleProvider>? providers = null)
    {
        ArgumentNullException.ThrowIfNull(api);

        return new RestMicroBundle(
            id,
            api,
            version,
            dependencies,
            providers);
    }
}
