using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Provides ecosystem composition helpers for REST API descriptors.</summary>
public static class RestApiDescriptorExtensions
{
    /// <summary>
    /// Creates a MicroBundle carrying the REST API capability.
    /// </summary>
    public static RestMicroBundle ToMicroBundle(
        this RestApiDescriptor api,
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
