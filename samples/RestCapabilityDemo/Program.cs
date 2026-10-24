using System.Net;
using System.Text;
using TheSingularityWorkshop.FSM_REST;

var operation = new RestOperationDescriptor(
    "GET",
    "/items/{id}",
    "getItem",
    "Get an item",
    "Fetch one item by identifier.",
    [new RestParameterDescriptor("id", "path", true, "string", null, "Item identifier.")],
    null,
    [new RestResponseDescriptor("200", "Item returned.", ["application/json"], "object", null)]);

var binding = new RestOperationBinding(
    operation,
    new Dictionary<string, string?> { ["id"] = "42" });

var request = RestRequestFactory.Create(binding, new Uri("https://example.test/v1"));
using var client = new HttpClient(new DemoHandler());
IRestTransport transport = new HttpClientRestTransport(client);
var response = await transport.SendAsync(request);

Console.WriteLine($"{(int)response.StatusCode} {response.ReasonPhrase}");
Console.WriteLine(response.Body);

if (!response.IsSuccessStatusCode || !response.Body.Contains("\"id\":42", StringComparison.Ordinal))
    return 1;

return 0;

internal sealed class DemoHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Method != HttpMethod.Get ||
            request.RequestUri?.AbsoluteUri != "https://example.test/v1/items/42")
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":42,\"name\":\"Ada\"}", Encoding.UTF8, "application/json")
        });
    }
}
