namespace TheSingularityWorkshop.FSM_REST;

public interface IRestTransport
{
    Task<RestResponse> SendAsync(
        RestRequest request,
        CancellationToken cancellationToken = default);
}
