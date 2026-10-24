using System.Net;
using System.Text;
using TheSingularityWorkshop.FSM_REST;
using TheSingularityWorkshop.FSM_REST.GUI;
using Xunit;

namespace TheSingularityWorkshop.FSM_REST.Tests;

public sealed class RestGuiOperationExecutorTests
{
    [Fact]
    public async Task ResolvesApplicationDataCallsEndpointAndPublishesResult()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/people/{id}", "getPerson", "Get person", null,
            [new RestParameterDescriptor("id", "path", true, "string", null, "Person identifier.")],
            null,
            [new RestResponseDescriptor("200", "OK", ["application/json"], "object", null)]);
        var api = new RestApiDescriptor("People API", "1", [operation]);
        var handler = new RoutingHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"id\":42,\"name\":\"Ada\"}", Encoding.UTF8, "application/json")
            });
        using var client = new HttpClient(handler);
        IRestTransport transport = new HttpClientRestTransport(client);
        var mapping = new RestGuiMapping(
            operation, "Open person", "detail",
            [new RestGuiInputMapping("id", "Person ID", "text", "selection.personId", Required: true)],
            resultTarget: "views.person");

        string? publishedPath = null;
        RestResponse? publishedResponse = null;
        var response = await RestGuiOperationExecutor.ExecuteAsync(
            api, new Uri("https://api.example.test/v1"), transport, mapping,
            path => path == "selection.personId" ? "42" : null,
            publishResult: (path, value) => { publishedPath = path; publishedResponse = value; });

        Assert.Equal(200, response.StatusCode);
        Assert.Equal("https://api.example.test/v1/people/42", handler.Requests.Single().AbsoluteUri);
        Assert.Equal("views.person", publishedPath);
        Assert.Same(response, publishedResponse);
        var person = response.Deserialize<Person>();
        Assert.NotNull(person);
        Assert.Equal(42, person.Id);
        Assert.Equal("Ada", person.Name);
    }

    [Fact]
    public async Task RejectsRequiredInputWhenDataSourceHasNoValue()
    {
        var operation = new RestOperationDescriptor(
            "GET", "/people/{id}", "getPerson", "Get person", null,
            [new RestParameterDescriptor("id", "path", true, "string", null, "Person identifier.")],
            null,
            [new RestResponseDescriptor("200", "OK", ["application/json"], "object", null)]);
        var api = new RestApiDescriptor("People API", "1", [operation]);
        var handler = new RoutingHandler(_ => new HttpResponseMessage(HttpStatusCode.OK));
        using var client = new HttpClient(handler);
        IRestTransport transport = new HttpClientRestTransport(client);
        var mapping = new RestGuiMapping(
            operation, "Open person", "detail",
            [new RestGuiInputMapping("id", "Person ID", "text", "selection.personId", Required: true)]);

        await Assert.ThrowsAsync<ArgumentException>(() => RestGuiOperationExecutor.ExecuteAsync(
            api, new Uri("https://api.example.test/v1"), transport, mapping, _ => null));
        Assert.Empty(handler.Requests);
    }

    private sealed record Person(int Id, string Name);

    private sealed class RoutingHandler(Func<HttpRequestMessage, HttpResponseMessage> route) : HttpMessageHandler
    {
        public List<Uri> Requests { get; } = [];
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request.RequestUri!);
            return Task.FromResult(route(request));
        }
    }
}
