namespace TheSingularityWorkshop.FSM_REST;

/// <summary>Defines the transport boundary between a REST capability and communication mechanism.</summary>
/// <remarks>Implementations may use HTTP, a test double, or another transport. FSM_REST depends only on this contract.</remarks>
public interface IRestTransport
{
    /// <summary>Sends a request and returns the observed response.</summary>
    /// <param name="request">Request to send.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The response observed by the transport.</returns>
    Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default);
}
